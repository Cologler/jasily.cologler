using System;

namespace Jasily.Diagnostics.AttributeTest
{
    public sealed class FloatRangeAttribute : TestAttribute
    {
        public FloatRangeAttribute(double min = double.MinValue, double max = double.MaxValue)
        {
            this.Min = min;
            this.Max = max;
        }

        public double Min { get; }

        public double Max { get; }

        protected override bool TestCore(object obj)
        {
            if (obj is int) return this.Test((int)obj);
            if (obj is long) return this.Test((long)obj);
            if (obj is decimal) return this.Test((decimal)obj);
            if (obj is float) return this.Test((float)obj);
            if (obj is double) return this.Test((double)obj);

            return false;
        }

        private bool Test(long number) => this.Test(Convert.ToDouble(number));

        private bool Test(int number) => this.Test(Convert.ToDouble(number));

        private bool Test(decimal number) => this.Test(Convert.ToDouble(number));

        private bool Test(float number) => this.Test(Convert.ToDouble(number));

        private bool Test(double number) => number >= this.Min && number <= this.Max;
    }
}