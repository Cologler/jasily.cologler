using System.Linq;

namespace System.Collections.Generic
{
    public static class ICollectionExtensions
    {
        public static void AddRange<T>(this ICollection<T> collection, params T[] items)
        {
            if (items == null) throw new ArgumentNullException(nameof(items));

            foreach (var item in items)
            {
                collection.Add(item);
            }
        }
        public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> items)
        {
            if (items == null) throw new ArgumentNullException(nameof(items));

            foreach (var item in items)
            {
                collection.Add(item);
            }
        }

        public static ICollection<T> Append<T>(this ICollection<T> collection, T item)
        {
            collection.Add(item);
            return collection;
        }
        public static ICollection<T> Append<T>(this ICollection<T> collection, params T[] items)
        {
            collection.AddRange(items);
            return collection;
        }
        public static ICollection<T> Append<T>(this ICollection<T> collection, IEnumerable<T> items)
        {
            collection.AddRange(items);
            return collection;
        }

        public static T AddAndReturn<T>(this ICollection<T> collection, T item)
        {
            collection.Add(item);
            return item;
        }
        public static IEnumerable<T> AddAndReturn<T>(this ICollection<T> collection, IEnumerable<T> items)
        {
            // ReSharper disable once PossibleMultipleEnumeration
            collection.AddRange(items);
            // ReSharper disable once PossibleMultipleEnumeration
            return items;
        }

        public static int RemoveRange<T>(this ICollection<T> collection, params T[] items)
        {
            return items.Count(collection.Remove);
        }
        public static int RemoveRange<T>(this ICollection<T> collection, IEnumerable<T> items)
        {
            return items.Count(collection.Remove);
        }

        public static bool MoveTo<T>(this ICollection<T> source, T item, ICollection<T> dest)
        {
            if (source.Remove(item))
            {
                dest.Add(item);
                return true;
            }

            return false;
        }
    }
}