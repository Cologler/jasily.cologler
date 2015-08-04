using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Jasily.Text
{
    public sealed class TextReplacer
    {
        private Comparer<KeyValueFinder> comparer;

        public TextReplacer(string originText)
        {
            if (originText == null) throw new ArgumentNullException(nameof(originText));

            this.OriginText = originText;
        }

        public string OriginText { get; }

        private int StartPointer;

        public string Replace(IReadOnlyDictionary<string, string> replacement)
        {
            if (replacement == null) throw new ArgumentNullException(nameof(replacement));
            
            if (this.OriginText.Length == 0) return string.Empty;

            Interlocked.CompareExchange(ref this.comparer,
                Comparer<KeyValueFinder>.Create(KeyValueFinder.Compare),
                null);

            var rl = replacement
                    .Select(z => new KeyValueFinder(this, z.Key, z.Value))
                    .Where(z => z.Current > -1)
                    .ToList();
            if (rl.Count == 0) return this.OriginText;
            rl.Sort(this.comparer);

            var sb = new StringBuilder();
            lock (this.comparer)
            {
                this.StartPointer = 0;

                while (true)
                {
                    if (rl.Count == 0) break;
                    var first = rl[0];

                    if (rl.Count > 1 && first.Current == rl[1].Current)
                        throw new ArgumentException($"key '{first.Key}' was conflict with key '{rl[1].Key}'");

                    if (first.Current > this.StartPointer)
                        sb.Append(this.OriginText, this.StartPointer, first.Current - this.StartPointer);
                    sb.Append(first.Value);
                    this.StartPointer = first.Current + first.Key.Length;

                    // resort
                    rl.Remove(first);
                    if (first.MoveNext())
                        rl.Insert(~rl.BinarySearch(first, this.comparer), first);
                }

                if (this.StartPointer < this.OriginText.Length - 1) sb.Append(this.OriginText, this.StartPointer, this.OriginText.Length - this.StartPointer);
            }
            return sb.ToString();
        }

        private class KeyValueFinder : IEnumerator<int>
        {
            public readonly string Key;
            public readonly string Value;
            private readonly TextReplacer parent;

            public KeyValueFinder(TextReplacer parent, string key, string value)
            {
                if (key.IsNullOrEmpty()) throw new ArgumentException("key can not be empty or null.");
                if (value == null) throw new ArgumentNullException(nameof(value));

                this.Key = key;
                this.Value = value;
                this.parent = parent;
                this.Current = -1;

                this.Reset();
                this.MoveNext();
            }

            /// <summary>
            /// 将枚举数推进到集合的下一个元素。
            /// </summary>
            /// <returns>
            /// 如果枚举数成功地推进到下一个元素，则为 true；如果枚举数越过集合的结尾，则为 false。
            /// </returns>
            /// <exception cref="T:System.InvalidOperationException">在创建了枚举数后集合被修改了。</exception>
            public bool MoveNext()
            {
                if (this.parent.StartPointer < this.Current) return true;
                var start = this.Current == -1 ? 0 : this.Current + this.Key.Length;
                if (start >= this.parent.OriginText.Length) return false;
                this.Current = this.parent.OriginText.IndexOf(this.Key, start, StringComparison.Ordinal);
                return this.Current != -1;
            }

            /// <summary>
            /// 将枚举数设置为其初始位置，该位置位于集合中第一个元素之前。
            /// </summary>
            /// <exception cref="T:System.InvalidOperationException">在创建了枚举数后集合被修改了。</exception>
            public void Reset() => this.Current = -1;

            /// <summary>
            /// 获取集合中位于枚举数当前位置的元素。
            /// </summary>
            /// <returns>
            /// 集合中位于枚举数当前位置的元素。
            /// </returns>
            public int Current { get; private set; }

            /// <summary>
            /// 获取集合中的当前元素。
            /// </summary>
            /// <returns>
            /// 集合中的当前元素。
            /// </returns>
            object IEnumerator.Current => this.Current;

            /// <summary>
            /// 执行与释放或重置非托管资源相关的应用程序定义的任务。
            /// </summary>
            public void Dispose()
            {
                
            }

            public static int Compare(KeyValueFinder x, KeyValueFinder y)
            {
                return x.Current.CompareTo(y.Current);
            }
        }
    }
}