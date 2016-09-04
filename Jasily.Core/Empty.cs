using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using JetBrains.Annotations;

namespace System
{
    public static class Empty<T>
    {
        public static readonly T[] Array = (T[])Linq.Enumerable.Empty<T>();

        public static IEnumerable<T> Enumerable => Linq.Enumerable.Empty<T>();

        static Empty()
        {
            Debug.Assert(Array != null);
            Debug.Assert(Enumerable != null);
        }

        public struct EmptyEnumerable : IEnumerable<T>
        {
            public EmptyEnumerator GetEnumerator() => new EmptyEnumerator();

            #region Implementation of IEnumerable

            /// <summary>返回一个循环访问集合的枚举器。</summary>
            /// <returns>可用于循环访问集合的 <see cref="T:System.Collections.Generic.IEnumerator`1" />。</returns>
            IEnumerator<T> IEnumerable<T>.GetEnumerator() => this.GetEnumerator();

            /// <summary>返回一个循环访问集合的枚举器。</summary>
            /// <returns>可用于循环访问集合的 <see cref="T:System.Collections.IEnumerator" /> 对象。</returns>
            IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

            #endregion

            public struct EmptyEnumerator : IEnumerator<T>
            {
                #region Implementation of IEnumerator

                /// <summary>将枚举数推进到集合的下一个元素。</summary>
                /// <returns>如果枚举数成功地推进到下一个元素，则为 true；如果枚举数越过集合的结尾，则为 false。</returns>
                /// <exception cref="T:System.InvalidOperationException">在创建了枚举数后集合被修改了。</exception>
                public bool MoveNext() => false;

                /// <summary>将枚举数设置为其初始位置，该位置位于集合中第一个元素之前。</summary>
                /// <exception cref="T:System.InvalidOperationException">在创建了枚举数后集合被修改了。</exception>
                public void Reset() { }

                /// <summary>获取集合中位于枚举数当前位置的元素。</summary>
                /// <returns>集合中位于枚举数当前位置的元素。</returns>
                public T Current => default(T);

                /// <summary>获取集合中的当前元素。</summary>
                /// <returns>集合中的当前元素。</returns>
                object IEnumerator.Current => this.Current;

                #endregion

                #region Implementation of IDisposable

                /// <summary>执行与释放或重置非托管资源相关的应用程序定义的任务。</summary>
                public void Dispose() { }

                #endregion
            }
        }
    }

    public static class Empty
    {
        #region null -> []

        public static T[] EmptyIfNull<T>(this T[] array) => array ?? Empty<T>.Array;

        public static IEnumerable<T> EmptyIfNull<T>(this IEnumerable<T> enumerable)
            => enumerable ?? Empty<T>.Enumerable;

        public static string EmptyIfNull<T>(this string str) => str ?? string.Empty;

        #endregion

        #region [] -> null

        [CanBeNull]
        public static List<T> NullIfEmpty<T>([CanBeNull] this List<T> item) => NullIfEmpty<List<T>, T>(item);

        [CanBeNull]
        public static T[] NullIfEmpty<T>([CanBeNull] this T[] item) => NullIfEmpty<T[], T>(item);

        [CanBeNull]
        public static TCollection NullIfEmpty<TCollection, T>([CanBeNull] TCollection item)
            where TCollection : class, ICollection<T>
            => item == null || item.Count == 0 ? null : item;

        #endregion
    }
}