using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Data.Torrent
{
    [DebuggerDisplay("[{Type}] {Value}")]
    internal struct BencodingList : IBencodingObject<IBencodingObject[]>
    {
        List<IBencodingObject> _value;

        internal BencodingList(BinaryReader reader)
        {
            _value = new List<IBencodingObject>();
            byte b;
            while ((b = reader.ReadByte()) != Bencoding.EndByte)
                _value.Add(Bencoding.Parse(b, reader));
        }

        public IBencodingObject[] Value
        {
            get { return _value.ToArray(); }
        }

        public BencodingObjectType Type
        {
            get { return BencodingObjectType.List; }
        }

        object IBencodingObject.Value
        {
            get { return this.Value; }
        }

        List<byte> IBencodingObject.WriteOriginBytes(List<byte> bytes)
        {
            bytes.Add((byte)this.Type);
            foreach (var item in _value)
                item.WriteOriginBytes(bytes);
            bytes.Add(Bencoding.EndByte);
            return bytes;
        }
    }
}
