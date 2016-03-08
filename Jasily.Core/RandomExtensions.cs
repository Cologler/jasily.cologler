namespace System
{
    public static class RandomExtensions
    {
        public static bool NextBoolean(this Random random) => random.Next(2) == 0;

        public static byte[] NextBytes(this Random random, int byteCount)
        {
            var buffer = new byte[byteCount];
            random.NextBytes(buffer);
            return buffer;
        }

        public static int NextInt32(this Random random) => BitConverter.ToInt32(random.NextBytes(4), 0);

        public static uint NextUInt32(this Random random) => BitConverter.ToUInt32(random.NextBytes(4), 0);

        public static long NextInt64(this Random random) => BitConverter.ToInt64(random.NextBytes(8), 0);

        public static ulong NextUInt64(this Random random) => BitConverter.ToUInt64(random.NextBytes(8), 0);
    }
}