using System;

namespace Jasily.Diagnostics.AttributeTest
{
    public sealed class IntRangeAttribute : TestAttribute
    {
        public IntRangeAttribute(long min = long.MinValue, long max = long.MaxValue)
        {
            this.Min = min;
            this.Max = max;
        }

        public long Min { get; }

        public long Max { get; }

        protected override bool TestCore(object obj)
        {
            if (obj is int) return this.Test((int)obj);
            if (obj is long) return this.Test((long)obj);
            if (obj is decimal) return this.Test((decimal)obj);
            if (obj is float) return this.Test((float)obj);
            if (obj is double) return this.Test((double)obj);

            return false;
        }

        private bool Test(long number) => number >= this.Min && number <= this.Max;

        private bool Test(int number) => this.Test(Convert.ToInt64(number));

        private bool Test(decimal number) => this.Test(Convert.ToInt64(number));

        private bool Test(float number) => this.Test(Convert.ToInt64(number));

        private bool Test(double number) => this.Test(Convert.ToInt64(number));
    }
}
