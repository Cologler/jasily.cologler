using System.Linq;

namespace System.Collections.Generic
{
    public static class JasilyList
    {
        public static int RemoveRange<T>(this List<T> self, IEnumerable<T> items)
        {
            return items.Count(self.Remove);
        }

    }
}