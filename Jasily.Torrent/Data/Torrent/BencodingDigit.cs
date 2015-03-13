using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace System.Data.Torrent
{
    [DebuggerDisplay("[{Type}] {Value}")]
    internal struct BencodingDigit : IBencodingObject<long>
    {
        List<byte> _originBytes;
        long _value;

        internal BencodingDigit(byte header, BinaryReader reader)
        {
            _originBytes = new List<byte>() { header };
            byte b;
            while ((b = reader.ReadByte()) != Bencoding.EndByte)
                _originBytes.Add(b);
            _value = Int64.Parse(Encoding.UTF8.GetString(_originBytes.ToArray(), 1, _originBytes.Count - 1));
            _originBytes.Add(b);
        }

        public long Value
        {
            get { return this._value; }
        }

        public BencodingObjectType Type
        {
            get { return BencodingObjectType.Digit; }
        }

        object IBencodingObject.Value
        {
            get { return this.Value; }
        }

        List<byte> IBencodingObject.WriteOriginBytes(List<byte> bytes)
        {
            bytes.AddRange(this._originBytes);
            return bytes;
        }
    }
}
