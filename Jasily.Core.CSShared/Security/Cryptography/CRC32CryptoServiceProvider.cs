
namespace System.Security.Cryptography
{
    // ReSharper disable once InconsistentNaming
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
            this.Polynomial = polynomial;
            this.Initialize();
        }

        private static uint[] BuildCRC32Table(uint polynomial)
        {
            uint crc;
            var table = new uint[256];

            for (var i = 0; i < 256; i++)
            {
                crc = (uint)i;
                for (var j = 8; j > 0; j--)
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
            this.Crc32Table = BuildCRC32Table(this.Polynomial);
            this.Crc32Value = AllOnes;
        }

        protected override void HashCore(byte[] buffer, int offset, int count)
        {
            ulong ptr;
            for (var i = offset; i < count; i++)
            {
                ptr = (this.Crc32Value & 0xFF) ^ buffer[i];
                this.Crc32Value >>= 8;
                this.Crc32Value ^= this.Crc32Table[ptr];
            }
        }

        protected override byte[] HashFinal()
        {
            var finalHash = new byte[4];
            ulong finalCRC = this.Crc32Value ^ AllOnes;

            finalHash[3] = (byte)((finalCRC >> 0) & 0xFF);
            finalHash[2] = (byte)((finalCRC >> 8) & 0xFF);
            finalHash[1] = (byte)((finalCRC >> 16) & 0xFF);
            finalHash[0] = (byte)((finalCRC >> 24) & 0xFF);

            return finalHash;
        }
    }
}
