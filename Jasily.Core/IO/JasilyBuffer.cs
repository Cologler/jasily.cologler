using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.IO
{
    public struct JasilyBuffer
    {
        public JasilyBuffer(byte[] buffer, int offset, int count)
        {
            this.Buffer = buffer;
            this.Offset = offset;
            this.Count = count;
        }

        public byte[] Buffer { get; }

        public int Offset { get; }

        public int Count { get; }
    }
}
