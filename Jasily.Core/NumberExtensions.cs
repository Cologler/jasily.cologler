namespace System
{
    public static class NumberExtensions
    {
        public static int? TryBinarySqrt(this int value)
        {
            if (value == 0) return null;
            if (value == 1) return 0;

            var n = 0;
            while (value != 0 && (value & 1) == 0)
            {
                if (value == 2) return 1 + n;
                if (value == 4) return 2 + n;
                if (value == 8) return 3 + n;
                if (value == 16) return 4 + n;
                if ((value & 0xf) != 0) return null;
                value >>= 4;
                n += 4;
            }
            return value == 0 ? n : (int?)null;
        }

        public static Tuple<int, long> SplitHighestPlace(this double value, int system)
        {
            if (system <= 0) throw new ArgumentOutOfRangeException(nameof(system));

            value = Math.Abs(value);
            if (value < 1) return Tuple.Create(0, 0L);

            while (value > long.MaxValue) value /= system;
            return SplitHighestPlace((long)value, system);
        }

        public static Tuple<int, long> SplitHighestPlace(this long value, int system)
        {
            if (system <= 0) throw new ArgumentOutOfRangeException(nameof(system));

            if (value == 0) return Tuple.Create(0, 0L);

            value = Math.Abs(value);
            var next = 1L;
            var x = TryBinarySqrt(system);
            if (x.HasValue)
            {
                do
                {
                    next <<= x.Value;
                } while (value >= next);
                next >>= x.Value;
            }
            else
            {
                do
                {
                    next *= system;
                } while (value >= next);
                next /= system;
            }
            return Tuple.Create((int)(value / next), value % next);
        }

        public static Tuple<int, long> SplitHighestPlace(this int value, int system)
            => SplitHighestPlace((long)value, system);
    }
}