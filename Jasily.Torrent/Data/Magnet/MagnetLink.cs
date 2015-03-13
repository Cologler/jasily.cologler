using System.Collections.Generic;
using System.Security.Cryptography;

namespace System.Data.Magnet
{
    public sealed class MagnetLink
    {
        List<IMagnetElement> MagnetElement;

        public MagnetLink(HashType hashType, string hash)
        {
            MagnetElement = new List<IMagnetElement>();

        }
    }
}
