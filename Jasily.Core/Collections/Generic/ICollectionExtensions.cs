using System.Linq;

namespace System.Collections.Generic
{
    public static class ICollectionExtensions
    {
        public static void AddRange<T>(this ICollection<T> collection, params T[] items)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            if (items == null) throw new ArgumentNullException(nameof(items));

            foreach (var item in items)
            {
                collection.Add(item);
            }
        }
        public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> items)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            if (items == null) throw new ArgumentNullException(nameof(items));

            foreach (var item in items)
            {
                collection.Add(item);
            }
        }

        public static ICollection<T> Append<T>(this ICollection<T> collection, T item)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));

            collection.Add(item);
            return collection;
        }
        public static ICollection<T> Append<T>(this ICollection<T> collection, params T[] items)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));

            collection.AddRange(items);
            return collection;
        }
        public static ICollection<T> Append<T>(this ICollection<T> collection, IEnumerable<T> items)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));

            collection.AddRange(items);
            return collection;
        }

        public static T AddAndReturn<T>(this ICollection<T> collection, T item)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));

            collection.Add(item);
            return item;
        }
        public static IEnumerable<T> AddAndReturn<T>(this ICollection<T> collection, IEnumerable<T> items)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));

            // ReSharper disable once PossibleMultipleEnumeration
            collection.AddRange(items);
            // ReSharper disable once PossibleMultipleEnumeration
            return items;
        }

        public static int RemoveRange<T>(this ICollection<T> collection, params T[] items)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));

            return items.Count(collection.Remove);
        }
        public static int RemoveRange<T>(this ICollection<T> collection, IEnumerable<T> items)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));

            return items.Count(collection.Remove);
        }

        public static bool MoveTo<T>(this ICollection<T> source, T item, ICollection<T> dest)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (dest == null) throw new ArgumentNullException(nameof(dest));

            return InnerMoveTo(source, item, dest);
        }
        public static int MoveTo<T>(this ICollection<T> source, IEnumerable<T> items, ICollection<T> dest)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (items == null) throw new ArgumentNullException(nameof(items));
            if (dest == null) throw new ArgumentNullException(nameof(dest));

            return items.Count(z => InnerMoveTo(source, z, dest));
        }
        private static bool InnerMoveTo<T>(ICollection<T> source, T item, ICollection<T> dest)
        {
            if (source.Remove(item))
            {
                dest.Add(item);
                return true;
            }

            return false;
        }

        public static void Reset<T>(this ICollection<T> collection, T item)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));

            collection.Clear();
            collection.Add(item);
        }
        public static void Reset<T>(this ICollection<T> collection, params T[] items)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            if (items == null) throw new ArgumentNullException(nameof(items));

            collection.Clear();
            collection.AddRange(items);
        }
        public static void Reset<T>(this ICollection<T> collection, IEnumerable<T> items)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            if (items == null) throw new ArgumentNullException(nameof(items));

            collection.Clear();
            collection.AddRange(items);
        }
    }
}