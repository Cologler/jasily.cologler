using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace System.Data.Magnet
{
    interface IMagnetElementHash : IMagnetElement
    {
        HashType Type { get; }
    }
}
