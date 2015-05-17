using System.Collections.Generic;

namespace System.Collections.ObjectModel
{
    public static class JasilyCollection
    {
        public static void AddRange<T>(this Collection<T> self, IEnumerable<T> collection)
        {
            foreach (var item in collection)
            {
                self.Add(item);
            }
        }
    }
}