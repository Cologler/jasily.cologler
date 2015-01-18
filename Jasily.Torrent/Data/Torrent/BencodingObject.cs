using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace System.Data.Torrent
{
    [DebuggerDisplay("[{Type}] {Object}")]
    public struct BencodingObject
    {
        const byte EndByte = 101;

        private byte[] _originBytes;
        public BencodingObjectType Type;
        public object Object;

        public byte[] OriginBytes
        {
            get { return GetOriginBytes(new List<byte>()).ToArray(); }
        }

        public BencodingObject this[string key]
        {
            get
            {
                if (this.Type != BencodingObjectType.Dictionary)
                    throw new InvalidOperationException();

                return (this.Object as List<KeyValuePair<BencodingObject, BencodingObject>>).Single(z => (z.Key.Object as string) == key).Value;
            }
        }

        private List<byte> GetOriginBytes(List<byte> bytes)
        {
            switch (this.Type)
            {
                case BencodingObjectType.Dictionary:
                    bytes.Add((byte)this.Type);
                    var dic = Object as List<KeyValuePair<BencodingObject, BencodingObject>>;
                    foreach (var item in dic)
                        item.Value.GetOriginBytes(item.Key.GetOriginBytes(bytes));
                    bytes.Add(EndByte);
                    break;
                    
                case BencodingObjectType.List:
                    bytes.Add((byte)this.Type);
                    var list = Object as List<BencodingObject>;
                    foreach (var item in list)
                        item.GetOriginBytes(bytes);
                    bytes.Add(EndByte);
                    break;

                case BencodingObjectType.Digit:
                case BencodingObjectType.String:
                    bytes.AddRange(this._originBytes);
                    break;

                default:
                    break;
            }

            return bytes;
        }

        public BencodingObject(BinaryReader reader)
            : this(reader.ReadByte(), reader)
        {
        }
        private BencodingObject(byte header, BinaryReader reader)
        {
            var bytes = new List<byte>() { header };
            byte b;

            switch ((BencodingObjectType)bytes[0])
            {
                case BencodingObjectType.Dictionary:
                    Type = BencodingObjectType.Dictionary;
                    var dic = new List<KeyValuePair<BencodingObject, BencodingObject>>();
                    while ((b = reader.ReadByte()) != EndByte)
                    {
                        var key = new BencodingObject(b, reader);
                        var value = new BencodingObject(reader);
                        dic.Add(new KeyValuePair<BencodingObject, BencodingObject>(key, value));
                    }
                    bytes.Add(b);
                    this.Object = dic;
                    break;                

                case BencodingObjectType.List:
                    Type = BencodingObjectType.List;
                    var list = new List<BencodingObject>();
                    while ((b = reader.ReadByte()) != EndByte)
                        list.Add(new BencodingObject(b, reader));
                    bytes.Add(b);
                    this.Object = list;
                    break;

                case BencodingObjectType.Digit:
                    Type = BencodingObjectType.Digit;
                    while ((b = reader.ReadByte()) != EndByte)
                        bytes.Add(b);
                    this.Object = Int64.Parse(Encoding.UTF8.GetString(bytes.ToArray(), 1, bytes.Count - 1));
                    bytes.Add(b);
                    break;

                default:
                    Type = BencodingObjectType.String;
                    while ((b = reader.ReadByte()) != 58)
                        bytes.Add(b);
                    bytes.Add(b);
                    var count = int.Parse(Encoding.UTF8.GetString(bytes.ToArray(), 0, bytes.Count - 1));
                    var buf = reader.ReadBytes(count);
                    bytes.AddRange(buf);
                    this.Object = Encoding.UTF8.GetString(buf);
                    break;
            }
            
            _originBytes = bytes.ToArray();
        }

        public override string ToString()
        {
            if (Object == null)
                return base.ToString();

            switch (Type)
            {
                case BencodingObjectType.Dictionary:
                    return String.Join(", ", (Object as List<KeyValuePair<BencodingObject, BencodingObject>>).Take(5).Select(z => z.Key));

                case BencodingObjectType.Digit:
                case BencodingObjectType.List:
                case BencodingObjectType.String:
                default:
                    return Object.ToString();
            }
        }

        public override bool Equals(object obj)
        {
            if (obj is BencodingObject)
            {
                var bo = (BencodingObject)obj;

                return this.Object.Equals(bo.Object);
            }

            return false;
        }
    }
}
