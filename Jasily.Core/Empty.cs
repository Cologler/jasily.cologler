using System.Collections.Generic;
using System.Diagnostics;
using JetBrains.Annotations;

namespace System
{
    public static class Empty<T>
    {
        [NotNull]
        public static readonly T[] Array = (T[])Linq.Enumerable.Empty<T>();

        [NotNull]
        public static IEnumerable<T> Enumerable => Linq.Enumerable.Empty<T>();

        static Empty()
        {
            Debug.Assert(Array != null);
            Debug.Assert(Enumerable != null);
        }
    }
}