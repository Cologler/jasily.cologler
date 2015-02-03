using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Security.Cryptography
{
    public static class JasilyHash
    {
        public static HashAlgorithm Create(HashType hash)
        {
            switch (hash)
            {
                case HashType.CRC32:
                    return CRC32.Create();

                case HashType.MD5:
                    return MD5.Create();

                case HashType.SHA256:
                    return SHA256.Create();

                case HashType.SHA384:
                    return SHA384.Create();

                case HashType.SHA512:
                    return SHA512.Create();

                case HashType.SHA1:
                default:
                    return SHA1.Create();
            }
        }

        public static string ConverterToHashString(this byte[] hashBytes)
        {
            return BitConverter.ToString(hashBytes).Replace("-", "");
        }

        public static string ComputeHashString(this HashAlgorithm hash, byte[] buffer)
        {
            return hash.ComputeHash(buffer).ConverterToHashString();
        }
        public static string ComputeHashString(this HashAlgorithm hash, Stream inputStream)
        {
            return hash.ComputeHash(inputStream).ConverterToHashString();
        }
        public static string ComputeHashString(this HashAlgorithm hash, byte[] buffer, int offset, int count)
        {
            return hash.ComputeHash(buffer, offset, count).ConverterToHashString();
        }
    }
}
