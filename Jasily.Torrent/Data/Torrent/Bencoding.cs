using System.Collections.Generic;
using System.IO;

namespace System.Data.Torrent
{
    public static class Bencoding
    {
        internal const byte EndByte = 101;

        public static IBencodingDictionary Parse(Stream torrentStream)
        {
            using (var reader = new BinaryReader(torrentStream))
                return (IBencodingDictionary)Parse(reader);
        }

        internal static IBencodingObject Parse(BinaryReader reader)
        {
            return Parse(reader.ReadByte(), reader);
        }
        internal static IBencodingObject Parse(byte header, BinaryReader reader)
        {
            switch ((BencodingObjectType)header)
            {
                case BencodingObjectType.Dictionary:
                    return new BencodingDictionary(reader);
                
                case BencodingObjectType.List:
                    return new BencodingList(reader);

                case BencodingObjectType.Digit:
                    return new BencodingDigit(header, reader);

                case BencodingObjectType.String:
                default:
                    return new BencodingString(header, reader);
            }
        }

        public static byte[] OriginBytes(this IBencodingObject obj)
        {
            return obj.WriteOriginBytes(new List<byte>()).ToArray();
        }
    }
}
