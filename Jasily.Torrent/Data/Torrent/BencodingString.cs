using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace System.Data.Torrent
{
    [DebuggerDisplay("[{Type}] {Value}")]
    internal struct BencodingString : IBencodingObject<string>
    {
        List<byte> _originBytes;
        string _value;

        internal BencodingString(byte header, BinaryReader reader)
        {
            _originBytes = new List<byte>() { header };
            byte b;
            while ((b = reader.ReadByte()) != 58)
                _originBytes.Add(b);
            _originBytes.Add(b);
            var count = int.Parse(Encoding.UTF8.GetString(_originBytes.ToArray(), 0, _originBytes.Count - 1));
            var buf = reader.ReadBytes(count);
            _originBytes.AddRange(buf);
            this._value = Encoding.UTF8.GetString(buf); 
        }

        public string Value
        {
            get { return _value; }
        }

        public BencodingObjectType Type
        {
            get { return BencodingObjectType.String; }
        }

        object IBencodingObject.Value
        {
            get { return this.Value; }
        }

        public List<byte> WriteOriginBytes(List<byte> bytes)
        {
            bytes.AddRange(this._originBytes);
            return bytes;
        }
    }
}
