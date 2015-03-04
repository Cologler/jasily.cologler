using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Security.Cryptography
{
    public sealed class CRC32CryptoServiceProvider : CRC32
    {
        const uint DefaultPolynomial = 0xEDB88320;
        const uint AllOnes = 0xffffffff;

        uint Polynomial;
        uint[] Crc32Table;
        uint Crc32Value;

        public CRC32CryptoServiceProvider()
            : this(DefaultPolynomial)
        {
        }

        public CRC32CryptoServiceProvider(uint polynomial)
        {
            Polynomial = polynomial;
            Initialize();
        }

        private static uint[] BuildCRC32Table(uint polynomial)
        {
            uint crc;
            uint[] table = new uint[256];

            for (int i = 0; i < 256; i++)
            {
                crc = (uint)i;
                for (int j = 8; j > 0; j--)
                {
                    if ((crc & 1) == 1)
                        crc = (crc >> 1) ^ polynomial;
                    else
                        crc >>= 1;
                }
                table[i] = crc;
            }

            return table;
        }

        public override void Initialize()
        {
            Crc32Table = CRC32CryptoServiceProvider.BuildCRC32Table(Polynomial);
            Crc32Value = AllOnes;
        }

        protected override void HashCore(byte[] buffer, int offset, int count)
        {
            ulong ptr;
            for (int i = offset; i < count; i++)
            {
                ptr = (Crc32Value & 0xFF) ^ buffer[i];
                Crc32Value >>= 8;
                Crc32Value ^= Crc32Table[ptr];
            }
        }

        protected override byte[] HashFinal()
        {
            byte[] finalHash = new byte[4];
            ulong finalCRC = Crc32Value ^ AllOnes;

            finalHash[3] = (byte)((finalCRC >> 0) & 0xFF);
            finalHash[2] = (byte)((finalCRC >> 8) & 0xFF);
            finalHash[1] = (byte)((finalCRC >> 16) & 0xFF);
            finalHash[0] = (byte)((finalCRC >> 24) & 0xFF);

            return finalHash;
        }
    }
}
