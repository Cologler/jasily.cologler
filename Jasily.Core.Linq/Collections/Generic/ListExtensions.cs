using JetBrains.Annotations;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace System.Collections.Generic
{
    public static class ListExtensions
    {
        #region IList

        public static IReadOnlyList<T> AsReadOnly<T>([NotNull] this IList<T> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            return new ReadOnlyCollection<T>(source);
        }

        public static IEnumerable<T> Skip<T>([NotNull] this IList<T> source, int count)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            for (var i = count; i < source.Count; i++) yield return source[i];
        }

        private static int InternalFindIndex<T>([NotNull] IList<T> source, int startIndex, int count, [NotNull] Predicate<T> match)
        {
            Debug.Assert(source != null);
            Debug.Assert(match != null);

            count = Math.Min(startIndex + count, source.Count);
            for (var i = startIndex; i < count; i++)
            {
                if (match(source[i])) return i;
            }
            return -1;
        }

        public static int FindIndex<T>([NotNull] this IList<T> source, int startIndex, int count, [NotNull] Predicate<T> match)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (match == null) throw new ArgumentNullException(nameof(match));

            var list = source as List<T>;
            if (list != null) return list.FindIndex(startIndex, count, match);

            var array = source as T[];
            if (array != null) return Array.FindIndex(array, startIndex, count, match);

            return InternalFindIndex(source, startIndex, count, match);
        }

        public static int FindIndex<T>([NotNull] this IList<T> source, int startIndex, [NotNull] Predicate<T> match)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (match == null) throw new ArgumentNullException(nameof(match));

            var list = source as List<T>;
            if (list != null) return list.FindIndex(startIndex, match);

            var array = source as T[];
            if (array != null) return Array.FindIndex(array, startIndex, match);

            return InternalFindIndex(source, startIndex, source.Count, match);
        }

        public static int FindIndex<T>([NotNull] this IList<T> source, [NotNull] Predicate<T> match)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (match == null) throw new ArgumentNullException(nameof(match));

            var list = source as List<T>;
            if (list != null) return list.FindIndex(match);

            var array = source as T[];
            if (array != null) return Array.FindIndex(array, match);

            return InternalFindIndex(source, 0, source.Count, match);
        }

        #endregion
    }
}