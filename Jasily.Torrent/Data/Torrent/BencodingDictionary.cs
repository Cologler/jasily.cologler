using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace System.Data.Torrent
{
    [DebuggerDisplay("[{Type}] {Value}")]
    internal struct BencodingDictionary : IBencodingDictionary
    {
        List<KeyValuePair<BencodingString, IBencodingObject>> _value;
        Dictionary<string, IBencodingObject> _dic;

        internal BencodingDictionary(BinaryReader reader)
        {
            _value = new List<KeyValuePair<BencodingString, IBencodingObject>>();
            _dic = new Dictionary<string, IBencodingObject>();

            byte b;
            while ((b = reader.ReadByte()) != Bencoding.EndByte)
            {
                var key = new BencodingString(b, reader);
                var value = Bencoding.Parse(reader);
                _value.Add(new KeyValuePair<BencodingString, IBencodingObject>(key, value));
            }
            foreach (var item in _value)
                _dic.Add(item.Key.Value, item.Value);
        }

        public Dictionary<string, IBencodingObject> Value
        {
            get
            {
                var dic = new Dictionary<string, IBencodingObject>();
                foreach (var item in _value)
                    dic.Add(item.Key.Value, item.Value);
                return dic;
            }
        }

        public BencodingObjectType Type
        {
            get { return BencodingObjectType.Dictionary; }
        }

        object IBencodingObject.Value
        {
            get { return this.Value; }
        }    

        public List<byte> WriteOriginBytes(List<byte> bytes)
        {
            bytes.Add((byte)this.Type);
            foreach (var item in _value)
                item.Value.WriteOriginBytes(item.Key.WriteOriginBytes(bytes));
            bytes.Add(Bencoding.EndByte);
            return bytes;
        }

        public bool ContainsKey(string key)
        {
            return _dic.ContainsKey(key);
        }

        public IEnumerable<string> Keys
        {
            get { return _dic.Keys; }
        }

        public bool TryGetValue(string key, out IBencodingObject value)
        {
            return _dic.TryGetValue(key, out value);
        }

        public IEnumerable<IBencodingObject> Values
        {
            get { return _dic.Values; }
        }

        public IBencodingObject this[string key]
        {
            get { return _dic[key]; }
        }

        public int Count
        {
            get { return _dic.Count; }
        }

        public IEnumerator<KeyValuePair<string, IBencodingObject>> GetEnumerator()
        {
            return _dic.GetEnumerator();
        }

        Collections.IEnumerator Collections.IEnumerable.GetEnumerator()
        {
            return _dic.GetEnumerator();
        }
    }
}
