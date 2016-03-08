using System.Collections.Generic;

namespace Jasily
{
    public static class Empty<T>
    {
        public static readonly T[] Array = new T[0];

        public static IEnumerable<T> Enumerable => System.Linq.Enumerable.Empty<T>();
    }
}