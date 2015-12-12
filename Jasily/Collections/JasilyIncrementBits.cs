using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Jasily.Collections
{
    public class JasilyIncrementBits : IEnumerable<JasilyIncrementBits.BitValue>
    {
        private readonly int length;
        private readonly ulong max;

        public int BitLength => this.length;

        public JasilyIncrementBits(int bitLength)
        {
            if (bitLength < 1) throw new ArgumentException("must > 0", nameof(bitLength));
            if (bitLength > 64) throw new ArgumentException("PC not support length > 64", nameof(bitLength));

            this.length = bitLength;
            this.max = (ulong)Math.Pow(2, this.length);
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