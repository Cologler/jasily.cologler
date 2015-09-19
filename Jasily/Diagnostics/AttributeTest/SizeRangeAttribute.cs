using System;
using System.Collections;

namespace Jasily.Diagnostics.AttributeTest
{
    public sealed class SizeRangeAttribute : TestAttribute
    {
        public SizeRangeAttribute(long min = long.MinValue, long max = long.MaxValue)
        {
            this.Min = min;
            this.Max = max;
        }

        public long Min { get; }

        public long Max { get; }

        public override bool Test(object obj)
        {
            throw new NotImplementedException();
        }

        private bool Test(int number) => this.Test(Convert.ToInt64(number));

        private bool Test(long number) => number >= this.Min && number <= this.Max;
    }
}
