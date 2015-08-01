using System;

#if WINDOWS_UWP
using Windows.Security.Cryptography.Core;
#endif

namespace Jasily.Security.Cryptography
{
    public static class JasilyHashFactory
    {
#if WINDOWS_UWP
        public static IJasilyHashProvider Create(HashType hash)
        {
            switch (hash)
            {
                case HashType.MD5:
                    return new UAPHashAlgorithmProvider(HashAlgorithmProvider.OpenAlgorithm(HashAlgorithmNames.Md5));
                case HashType.SHA1:
                    return new UAPHashAlgorithmProvider(HashAlgorithmProvider.OpenAlgorithm(HashAlgorithmNames.Sha1));
                case HashType.SHA256:
                    return new UAPHashAlgorithmProvider(HashAlgorithmProvider.OpenAlgorithm(HashAlgorithmNames.Sha256));
                case HashType.SHA384:
                    return new UAPHashAlgorithmProvider(HashAlgorithmProvider.OpenAlgorithm(HashAlgorithmNames.Sha384));
                case HashType.SHA512:
                    return new UAPHashAlgorithmProvider(HashAlgorithmProvider.OpenAlgorithm(HashAlgorithmNames.Sha512));

                case HashType.CRC32:
                case HashType.BitTorrentInfoHash:
                    throw new NotSupportedException();

                default:
                    throw new ArgumentOutOfRangeException(nameof(hash), hash, null);
            }
        }
#else
        public static IJasilyHashProvider Create(HashType hash)
        {
            switch (hash)
            {
                case HashType.CRC32:
                    break;
                case HashType.MD5:
                    break;
                case HashType.SHA1:
                    break;
                case HashType.SHA256:
                    break;
                case HashType.SHA384:
                    break;
                case HashType.SHA512:
                    break;
                case HashType.BitTorrentInfoHash:
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(hash), hash, null);
            }

            throw new NotSupportedException();
        }
#endif
    }
}
