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

    public static class Empty
    {
        public static T[] EmptyIfNull<T>(this T[] array) => array ?? Empty<T>.Array;

        public static IEnumerable<T> EmptyIfNull<T>(this IEnumerable<T> enumerable) => enumerable ?? Empty<T>.Enumerable;

        public static string EmptyIfNull<T>(this string str) => str ?? string.Empty;
    }
}