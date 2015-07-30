using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Diagnostics
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
            if (ReferenceEquals(obj, null)) return false;

            return obj.TryCast<int>()?.Select(this.Test)
                ?? obj.TryCast<long>()?.Select(this.Test)
                ?? (obj as Array)?.Length.Select(this.Test)
                ?? (obj as ICollection)?.Count.Select(this.Test)
                ?? false;
        }

        private bool Test(int number)
        {
            return this.Test(System.Convert.ToInt64(number));
        }
        private bool Test(long number)
        {
            return number >= this.Min && number <= this.Max;
        }
    }
}
