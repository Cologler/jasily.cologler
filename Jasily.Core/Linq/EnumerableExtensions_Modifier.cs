using System.Collections.Generic;
using JetBrains.Annotations;

namespace System.Linq
{
    public static partial class EnumerableExtensions
    {
        #region insert

        [PublicAPI]
        public static IEnumerable<T> Insert<T>([NotNull] this IEnumerable<T> source, int index, T item)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            var i = 0;
            using (var itor = source.GetEnumerator())
            {
                while (i < index && itor.MoveNext())
                {
                    yield return itor.Current;
                    i++;
                }

                if (i == index)
                {
                    yield return item;
                }
                else
                {
                    throw new ArgumentOutOfRangeException();
                }

                while (itor.MoveNext())
                {
                    yield return itor.Current;
                }
            }
        }

        [PublicAPI]
        public static IEnumerable<T> InsertToEnd<T>([NotNull] this IEnumerable<T> source, T next)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            foreach (var item in source) yield return item;
            yield return next;
        }

        [PublicAPI]
        public static IEnumerable<T> InsertToStart<T>([NotNull] this IEnumerable<T> source, T next)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            yield return next;
            foreach (var item in source) yield return item;
        }

        #endregion

        #region set

        [PublicAPI]
        public static IEnumerable<T> Set<T>([NotNull] this IEnumerable<T> source, int index, T item)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            var i = 0;
            using (var itor = source.GetEnumerator())
            {
                while (i < index && itor.MoveNext())
                {
                    yield return itor.Current;
                    i++;
                }

                if (i == index && itor.MoveNext())
                {
                    yield return item;
                }
                else
                {
                    throw new ArgumentOutOfRangeException();
                }

                while (itor.MoveNext())
                {
                    yield return itor.Current;
                }
            }
        }

        #endregion

        #region join

        public static IEnumerable<T> JoinWith<T>(this IEnumerable<T> source, T spliter)
        {
            using (var itor = source.GetEnumerator())
            {
                if (!itor.MoveNext()) yield break;
                while (true)
                {
                    yield return itor.Current;
                    if (!itor.MoveNext()) yield break;
                    yield return spliter;
                }
            }
        }

        public static IEnumerable<T> JoinWith<T>(this IEnumerable<T> source, Func<T> spliterFunc)
        {
            using (var itor = source.GetEnumerator())
            {
                if (!itor.MoveNext()) yield break;
                while (true)
                {
                    yield return itor.Current;
                    if (!itor.MoveNext()) yield break;
                    yield return spliterFunc();
                }
            }
        }

        public static IEnumerable<T> JoinWith<T>(this IEnumerable<T> source, Action action)
        {
            using (var itor = source.GetEnumerator())
            {
                if (!itor.MoveNext()) yield break;
                while (true)
                {
                    yield return itor.Current;
                    if (!itor.MoveNext()) yield break;
                    action();
                }
            }
        }

        #endregion
    }
}