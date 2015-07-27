using System.Collections.Generic;
using System.Linq;

namespace System.Collections.ObjectModel
{
    public static class CollectionExtensions
    {
        public static void AddRange<T>(this Collection<T> self, IEnumerable<T> collection)
        {
            foreach (var item in collection)
            {
                self.Add(item);
            }
        }

        public static int Remove<T>(this Collection<T> self, IEnumerable<T> collection)
        {
            return collection.Count(self.Remove);
        }
    }
}