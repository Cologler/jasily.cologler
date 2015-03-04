using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Security.Cryptography
{
    public abstract class CRC32 : HashAlgorithm
    {
        public static CRC32 Create()
        {
            return new CRC32CryptoServiceProvider();
        }
    }
}
