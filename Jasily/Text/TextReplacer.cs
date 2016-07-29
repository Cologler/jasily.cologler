using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Jasily.Collections.Generic;
using JetBrains.Annotations;

namespace Jasily.Text
{
    public sealed class TextReplacer
    {
        public static string Replace([NotNull] string text, [NotNull] IReadOnlyDictionary<string, string> replacements,
            StringComparison comparison = StringComparison.Ordinal)
        {
            if (text == null) throw new ArgumentNullException(nameof(text));
            if (replacements == null) throw new ArgumentNullException(nameof(replacements));

            if (text.Length == 0 || replacements.Count == 0) return text;

            var comparer = new CachedLastEqualityComparer<string>(JasilyComparer.GetStringComparer(comparison));
            var hashSet = new HashSet<string>(comparer);
            var replacer = new TextReplacer(text, comparison);
            var finders = new List<KeyValueFinder>(replacements.Count);
            foreach (var pair in replacements)
            {
                if (string.IsNullOrEmpty(pair.Key))
                    throw new ArgumentException("replacement source can not be empty", nameof(replacements));
                if (pair.Value == null)
                    throw new ArgumentNullException(nameof(replacements), "replacement dest can not be empty");
                if (!hashSet.Add(pair.Key))
                    throw new ArgumentException($"replacement source [{comparer.LastCompareItem1}] was conflict with [{comparer.LastCompareItem2}]",
                        nameof(replacements));

                finders.Add(new KeyValueFinder(replacer, pair.Key, pair.Value));
            }
            return replacer.Replace(finders);
        }

        private TextReplacer([NotNull] string originText, StringComparison comparison)
        {
            this.originText = originText;
            this.comparison = comparison;
        }

        [NotNull]
        private readonly string originText;

        private readonly StringComparison comparison;

        private int startIndex;

        private string Replace(List<KeyValueFinder> finders)
        {
            foreach (var finder in finders)
            {
                finder.Reset();
                finder.MoveNext();
            }
            finders.RemoveAll(z => z.Current < 0);
            if (finders.Count == 0) return this.originText;
            finders.Sort(KeyValueFinder.Comparer);

            var sb = new StringBuilder();
            while (true)
            {
                if (finders.Count == 0) break;

                var first = finders[0];

                // add replacement
                if (first.Current > this.startIndex) sb.Append(this.originText, this.startIndex, first.Current - this.startIndex);
                sb.Append(first.To);
                this.startIndex = first.Current + first.From.Length;

                // resort
                foreach (var finder in finders.ToArray())
                {
                    if (finder.Current < this.startIndex)
                    {
                        finders.Remove(finder);
                        if (finder.MoveNext()) finders.Insert(~finders.BinarySearch(finder, KeyValueFinder.Comparer), finder);
                    }
                }
            }
            return sb.Append(this.originText, this.startIndex).ToString();
        }

        private class KeyValueFinder : IEnumerator<int>
        {
            [NotNull]
            public readonly string From;
            [NotNull]
            public readonly string To;
            [NotNull]
            private readonly TextReplacer parent;

            public KeyValueFinder([NotNull] TextReplacer parent, [NotNull] string from, [NotNull] string to)
            {
                this.From = from;
                this.To = to;
                this.parent = parent;

                this.Reset();
                this.MoveNext();
            }

            public bool MoveNext()
            {
                if (this.parent.originText.Length - this.parent.startIndex < this.From.Length) return false;
                if (this.parent.startIndex <= this.Current) return true; // last time
                var start = this.Current < 0 ? 0 : this.Current + this.From.Length;
                if (start >= this.parent.originText.Length) return false;
                this.Current = this.parent.originText.IndexOf(this.From, start, this.parent.comparison);
                return this.Current >= 0;
            }

            public void Reset() => this.Current = -1;

            public int Current { get; private set; }

            object IEnumerator.Current => this.Current;

            public void Dispose() => this.Current = -1;

            private static int Compare(KeyValueFinder x, KeyValueFinder y) => x.Current == y.Current
                ? y.From.Length.CompareTo(x.From.Length)
                : x.Current.CompareTo(y.Current);

            public static readonly Comparer<KeyValueFinder> Comparer = Comparer<KeyValueFinder>.Create(Compare);
        }
    }
}