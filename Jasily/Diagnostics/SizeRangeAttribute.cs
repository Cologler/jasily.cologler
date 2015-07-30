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

            if (obj is int)
            {
                return Test((int)obj);
            }

            if (obj is long)
            {
                return Test((long)obj);
            }

            var array = obj as Array;
            if (array != null)
            {
                return Test(array.Length);
            }

            var col = obj as ICollection;
            if (col != null)
            {
                return Test(col.Count);
            }

            return false;
        }

        private bool Test(long number)
        {
            return number >= this.Min && number <= this.Max;
        }
    }
}
