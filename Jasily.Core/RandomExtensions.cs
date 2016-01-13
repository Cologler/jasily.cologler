using System.Collections.Generic;
using System.Linq;

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

        public static IEnumerable<T> RandomSort<T>(this IEnumerable<T> source)
        {
            var array = source.ToArray();
            var count = array.Length;
            var random = new Random();
            while (count > 0)
            {
                var index = random.Next(count);
                yield return array[index];
                array[index] = array[count - 1];
                array[count - 1] = default(T);
                count--;
            }
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