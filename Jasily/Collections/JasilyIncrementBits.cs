using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Jasily.Collections
{
    public class JasilyIncrementBits : IEnumerable<JasilyIncrementBits.BitValue>
    {
        private readonly ulong max;

        public int BitWidth { get; }

        public JasilyIncrementBits(int bitWidth)
        {
            if (bitWidth < 1) throw new ArgumentException("must > 0", nameof(bitWidth));
            if (bitWidth > 64) throw new ArgumentException("PC not support length > 64", nameof(bitWidth));

            this.BitWidth = bitWidth;
            this.max = (ulong)Math.Pow(2, this.BitWidth);
        }

        public IEnumerator<BitValue> GetEnumerator()
        {
            for (ulong i = 0; i < this.max; i++)
            {
                yield return new BitValue(i);
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

        public struct BitValue
        {
            private readonly ulong value;

            public BitValue(ulong value)
            {
                this.value = value;
            }

            public bool this[int index] => ((this.value >> (index % 64)) & 1) == 1;

            public IEnumerable<T> FilterItems<T>(IEnumerable<T> items)
            {
                var t = this;
                return items.Where((z, i) => t[i]);
            }
        }

        public static IEnumerable<IEnumerable<T>> SelectItems<T>(IEnumerable<T> items)
        {
            var itemsArray = items.ToArray();
            var ib = new JasilyIncrementBits(itemsArray.Length);
            return ib.Select(b => b.FilterItems(itemsArray));
        }
    }
}