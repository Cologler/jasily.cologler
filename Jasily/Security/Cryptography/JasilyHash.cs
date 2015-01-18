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
        public static string ConverterToHashString(this byte[] hashBytes)
        {
            return BitConverter.ToString(hashBytes).Replace("-", "").ToUpper();
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
