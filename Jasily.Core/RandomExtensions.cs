using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace System
{
    public static class RandomExtensions
    {
        public static bool NextBoolean([NotNull] this Random random) => random.Next(2) == 0;

        public static byte[] NextBytes([NotNull] this Random random, int byteCount)
        {
            var buffer = new byte[byteCount];
            random.NextBytes(buffer);
            return buffer;
        }

        public static int NextInt32([NotNull] this Random random) => BitConverter.ToInt32(random.NextBytes(4), 0);

        public static uint NextUInt32([NotNull] this Random random) => BitConverter.ToUInt32(random.NextBytes(4), 0);

        public static long NextInt64([NotNull] this Random random) => BitConverter.ToInt64(random.NextBytes(8), 0);

        public static ulong NextUInt64([NotNull] this Random random) => BitConverter.ToUInt64(random.NextBytes(8), 0);

        /// <summary>
        /// 根据权重返回索引。
        /// 如果数量为 0 或权重为 0，返回 -1。
        /// </summary>
        /// <param name="random"></param>
        /// <param name="elements"></param>
        /// <returns></returns>
        public static int Fall([NotNull] this Random random, [NotNull]  IReadOnlyList<int> elements)
        {
            if (elements.Count == 0) return -1;
            var sum = elements.Sum();
            if (sum == 0) return -1;
            var dest = random.Next(sum);
            for (var i = 0; i < elements.Count; i++)
            {
                if (elements[i] <= dest)
                {
                    dest -= elements[i];
                }
                else
                {
                    return i;
                }
            }
            return elements.Count - 1;
        }
    }
}