using System.IO;

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
                    return SHA1.Create();

                default:
                    throw new NotSupportedException("not support hash algorithm :" + hash.ToString());
            }
        }

        private static string ConverterToHexString(this byte[] hashBytes)
        {
            return BitConverter.ToString(hashBytes).Replace("-", "");
        }

        public static string ComputeHashString(this HashAlgorithm hash, string text)
        {
            return hash.ComputeHash(text.GetBytes()).ConverterToHashString();
        }
        public static string ComputeHashString(this HashAlgorithm hash, byte[] buffer)
        {
            return hash.ComputeHash(buffer).ConverterToHexString();
        }
        public static string ComputeHashString(this HashAlgorithm hash, Stream inputStream)
        {
            return hash.ComputeHash(inputStream).ConverterToHexString();
        }
        public static string ComputeHashString(this HashAlgorithm hash, byte[] buffer, int offset, int count)
        {
            return hash.ComputeHash(buffer, offset, count).ConverterToHexString();
        }
    }
}
