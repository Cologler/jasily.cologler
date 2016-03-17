using System.Collections.Generic;
using System.Diagnostics;

namespace System
{
    public static class Empty<T>
    {
        public static readonly T[] Array = (T[])Linq.Enumerable.Empty<T>();

        public static IEnumerable<T> Enumerable => Linq.Enumerable.Empty<T>();

        static Empty()
        {
            Debug.Assert(Array != null);
            Debug.Assert(Enumerable != null);
        }
    }
}