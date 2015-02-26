using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

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
