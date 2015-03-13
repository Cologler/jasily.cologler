using System.Security.Cryptography;

namespace System.Data.Magnet
{
    struct MagnetElementHash : IMagnetElementHash
    {
        const string NodeName = "urn";
        const string BitTorrentInfoHashName = "btih";
        const string SHA1HashName = "sha1";

        HashType Type;
        string Hash;

        public MagnetElementHash(HashType type, string hash)
        {
            Type = type;
            Hash = hash;

            switch (type)
            {
                case HashType.SHA1:
                case HashType.BitTorrentInfoHash:
                    break;

                default:
                    throw NotSupportedHashAlgorithmException();
            }
        }

        string IMagnetElement.NodeName
        {
            get { return NodeName; }
        }

        string IMagnetElement.NodeValue
        {
            get { return Hash; }
        }

        string IMagnetElement.AsMagnetElement()
        {
            return String.Join(":", NodeName, HashName, Hash);
        }

        HashType IMagnetElementHash.Type
        {
            get { return Type; }
        }

        string HashName
        {
            get
            {
                switch (this.Type)
                {
                    case HashType.SHA1:
                        return SHA1HashName;

                    case HashType.BitTorrentInfoHash:
                        return BitTorrentInfoHashName;

                    default:
                        throw NotSupportedHashAlgorithmException();
                }
            }
        }

        NotSupportedException NotSupportedHashAlgorithmException()
        {
            return new NotSupportedException("not support hash algorithm :" + Type.ToString());
        }        
    }
}
