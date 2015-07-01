using System.Collections.Generic;

namespace System
{
    public static class RandomExtensions
    {
        public static T Random<T>(this T[] t)
        {
            return t[new Random().Next(t.Length)];
        }
        public static T Random<T>(this IList<T> t)
        {
            return t[new Random().Next(t.Count)];
        }
    }
}