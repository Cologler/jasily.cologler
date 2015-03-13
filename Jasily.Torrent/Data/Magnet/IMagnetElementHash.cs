using System.Security.Cryptography;

namespace System.Data.Magnet
{
    interface IMagnetElementHash : IMagnetElement
    {
        HashType Type { get; }
    }
}
