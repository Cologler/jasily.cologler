using JetBrains.Annotations;
using System.Linq;

namespace System.Collections.Generic
{
    public static class CollectionExtensions
    {
        #region ICollection

        #region add or remove range

        public static ICollection<T> AddRange<T>([NotNull] this ICollection<T> collection, [NotNull] params T[] items)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            if (items == null) throw new ArgumentNullException(nameof(items));

            foreach (var item in items) collection.Add(item);
            return collection;
        }
        public static ICollection<T> AddRange<T>([NotNull] this ICollection<T> collection, [NotNull] IEnumerable<T> items)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            if (items == null) throw new ArgumentNullException(nameof(items));

            foreach (var item in items) collection.Add(item);
            return collection;
        }

        public static int RemoveRange<T>([NotNull] this ICollection<T> collection, [NotNull] params T[] items)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            if (items == null) throw new ArgumentNullException(nameof(items));

            return items.Count(collection.Remove);
        }
        public static int RemoveRange<T>([NotNull] this ICollection<T> collection, [NotNull] IEnumerable<T> items)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            if (items == null) throw new ArgumentNullException(nameof(items));

            return items.Count(collection.Remove);
        }

        #endregion

        public static ICollection<T> Append<T>([NotNull] this ICollection<T> collection, T item)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));

            collection.Add(item);
            return collection;
        }

        #region move

        public static bool MoveTo<T>([NotNull] this ICollection<T> source, T item,
            [NotNull] ICollection<T> dest)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (dest == null) throw new ArgumentNullException(nameof(dest));

            return InnerMoveTo(source, item, dest);
        }

        public static int MoveTo<T>([NotNull] this ICollection<T> source, [NotNull] IEnumerable<T> items,
            [NotNull] ICollection<T> dest)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (items == null) throw new ArgumentNullException(nameof(items));
            if (dest == null) throw new ArgumentNullException(nameof(dest));

            return items.Count(z => InnerMoveTo(source, z, dest));
        }

        private static bool InnerMoveTo<T>([NotNull] ICollection<T> source, T item, [NotNull] ICollection<T> dest)
        {
            if (source.Remove(item))
            {
                dest.Add(item);
                return true;
            }

            return false;
        }

        #endregion

        #region reset

        public static void Reset<T>([NotNull] this ICollection<T> collection, T item)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));

            collection.Clear();
            collection.Add(item);
        }
        public static void Reset<T>([NotNull] this ICollection<T> collection, [NotNull] params T[] items)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            if (items == null) throw new ArgumentNullException(nameof(items));

            collection.Clear();
            collection.AddRange(items);
        }
        public static void Reset<T>([NotNull] this ICollection<T> collection, [NotNull] IEnumerable<T> items)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            if (items == null) throw new ArgumentNullException(nameof(items));

            collection.Clear();
            collection.AddRange(items);
        }

        #endregion

        #endregion
    }
}