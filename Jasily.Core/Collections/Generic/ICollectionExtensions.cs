using System.Linq;

namespace System.Collections.Generic
{
    public static class ICollectionExtensions
    {
        public static void AddRange<T>(this ICollection<T> collection, params T[] items)
        {
            foreach (var item in items)
            {
                collection.Add(item);
            }
        }
        public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                collection.Add(item);
            }
        }
        
        public static int RemoveRange<T>(this ICollection<T> collection, params T[] items)
        {
            return items.Count(collection.Remove);
        }
        public static int RemoveRange<T>(this ICollection<T> collection, IEnumerable<T> items)
        {
            return items.Count(collection.Remove);
        }
    }
}