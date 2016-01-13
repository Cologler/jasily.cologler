using System.Collections.Generic;
using System.Linq;

namespace System
{
    public static class RandomExtensions
    {
        private static Random randomNumberGenerator;

        public static Random RandomNumberGenerator
            => randomNumberGenerator ?? (randomNumberGenerator = new Random());

        #region extension for collection

        public static T Random<T>(this T[] t, Random random = null)
        {
            if (t.Length == 0) return default(T);
            if (t.Length == 1) return t[0];
            random = random ?? RandomNumberGenerator;
            return t[random.Next(t.Length)];
        }

        public static T Random<T>(this IList<T> t, Random random = null)
        {
            if (t.Count == 0) return default(T);
            if (t.Count == 1) return t[0];
            random = random ?? RandomNumberGenerator;
            return t[random.Next(t.Count)];
        }

        public static IEnumerable<T> RandomSort<T>(this IEnumerable<T> source, Random random = null)
        {
            var array = source.ToArray();
            var count = array.Length;
            random = random ?? RandomNumberGenerator;
            while (count > 0)
            {
                var index = random.Next(count);
                yield return array[index];
                array[index] = array[count - 1];
                array[count - 1] = default(T);
                count--;
            }
        }

        #endregion

        public static byte[] NextBytes(this Random random, int byteCount)
        {
            var buffer = new byte[byteCount];
            random.NextBytes(buffer);
            return buffer;
        }

        public static int NextInt32(this Random random) => BitConverter.ToInt32(random.NextBytes(4), 0);

        public static long NextInt64(this Random random) => BitConverter.ToInt64(random.NextBytes(8), 0);
    }
}