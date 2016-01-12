using System.Collections.Generic;

namespace System
{
    public static class RandomExtensions
    {
        public static T Random<T>(this T[] t)
        {
            if (t.Length == 0) return default(T);
            if (t.Length == 1) return t[0];
            return t[new Random().Next(t.Length)];
        }

        public static T Random<T>(this IList<T> t)
        {
            if (t.Count == 0) return default(T);
            if (t.Count == 1) return t[0];
            return t[new Random().Next(t.Count)];
        }

        public static byte[] NextBytes(this Random random, int byteCount)
        {
            var buffer = new byte[byteCount];
            random.NextBytes(buffer);
            return buffer;
        }

        public static long NextInt32(Random random) => BitConverter.ToInt32(random.NextBytes(4), 0);

        public static long NextInt64(Random random) => BitConverter.ToInt64(random.NextBytes(8), 0);
    }
}