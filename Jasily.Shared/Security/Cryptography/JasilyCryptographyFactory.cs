using System;

#if WINDOWS_UWP
using Windows.Security.Cryptography.Core;
#endif

namespace Jasily.Security.Cryptography
{
    public static class JasilyCryptographyFactory
    {
#if WINDOWS_UWP
        private static string ConvertToHashAlgorithmName(HashAlgorithmType algorithmType)
        {
            switch (algorithmType)
            {
                case HashAlgorithmType.MD5:
                    return HashAlgorithmNames.Md5;
                case HashAlgorithmType.SHA1:
                    return HashAlgorithmNames.Sha1;
                case HashAlgorithmType.SHA256:
                    return HashAlgorithmNames.Sha256;
                case HashAlgorithmType.SHA384:
                    return HashAlgorithmNames.Sha384;
                case HashAlgorithmType.SHA512:
                    return HashAlgorithmNames.Sha512;

                default:
                    throw new NotSupportedException();
            }
        }

        public static IJasilyHashAlgorithmProvider CreateHash(HashAlgorithmType algorithmType)
            => new UAPHashAlgorithmProvider(
                    HashAlgorithmProvider.OpenAlgorithm(
                        ConvertToHashAlgorithmName(algorithmType)));

        private static string ConvertToAsymmetricKeyAlgorithmName(AsymmetricAlgorithmType algorithmType)
        {
            switch (algorithmType)
            {
                case AsymmetricAlgorithmType.DsaSha1:
                    return AsymmetricAlgorithmNames.DsaSha1;
                case AsymmetricAlgorithmType.DsaSha256:
                    return AsymmetricAlgorithmNames.DsaSha256;
                case AsymmetricAlgorithmType.EcdsaP256Sha256:
                    return AsymmetricAlgorithmNames.EcdsaP256Sha256;
                case AsymmetricAlgorithmType.EcdsaP384Sha384:
                    return AsymmetricAlgorithmNames.EcdsaP384Sha384;
                case AsymmetricAlgorithmType.EcdsaP521Sha512:
                    return AsymmetricAlgorithmNames.EcdsaP521Sha512;
                case AsymmetricAlgorithmType.EcdsaSha256:
                    return AsymmetricAlgorithmNames.EcdsaSha256;
                case AsymmetricAlgorithmType.EcdsaSha384:
                    return AsymmetricAlgorithmNames.EcdsaSha384;
                case AsymmetricAlgorithmType.EcdsaSha512:
                    return AsymmetricAlgorithmNames.EcdsaSha512;
                case AsymmetricAlgorithmType.RsaOaepSha1:
                    return AsymmetricAlgorithmNames.RsaOaepSha1;
                case AsymmetricAlgorithmType.RsaOaepSha256:
                    return AsymmetricAlgorithmNames.RsaOaepSha256;
                case AsymmetricAlgorithmType.RsaOaepSha384:
                    return AsymmetricAlgorithmNames.RsaOaepSha384;
                case AsymmetricAlgorithmType.RsaOaepSha512:
                    return AsymmetricAlgorithmNames.RsaOaepSha512;
                case AsymmetricAlgorithmType.RsaPkcs1:
                    return AsymmetricAlgorithmNames.RsaPkcs1;
                case AsymmetricAlgorithmType.RsaSignPkcs1Sha1:
                    return AsymmetricAlgorithmNames.RsaSignPkcs1Sha1;
                case AsymmetricAlgorithmType.RsaSignPkcs1Sha256:
                    return AsymmetricAlgorithmNames.RsaSignPkcs1Sha256;
                case AsymmetricAlgorithmType.RsaSignPkcs1Sha384:
                    return AsymmetricAlgorithmNames.RsaSignPkcs1Sha384;
                case AsymmetricAlgorithmType.RsaSignPkcs1Sha512:
                    return AsymmetricAlgorithmNames.RsaSignPkcs1Sha512;
                case AsymmetricAlgorithmType.RsaSignPssSha1:
                    return AsymmetricAlgorithmNames.RsaSignPssSha1;
                case AsymmetricAlgorithmType.RsaSignPssSha256:
                    return AsymmetricAlgorithmNames.RsaSignPssSha256;
                case AsymmetricAlgorithmType.RsaSignPssSha384:
                    return AsymmetricAlgorithmNames.RsaSignPssSha384;
                case AsymmetricAlgorithmType.RsaSignPssSha512:
                    return AsymmetricAlgorithmNames.RsaSignPssSha512;

                default:
                    throw new NotSupportedException();
            }
        }

        public static IJasilyAsymmetricKeyAlgorithmProvider CreateAsymmetric(AsymmetricAlgorithmType algorithmType)
            => new UAPAsymmetricKeyAlgorithmProvider(
                    AsymmetricKeyAlgorithmProvider.OpenAlgorithm(
                        ConvertToAsymmetricKeyAlgorithmName(algorithmType)));

        private static string ConvertToSymmetricKeyAlgorithmName(SymmetricAlgorithmType algorithmType)
        {
            switch (algorithmType)
            {
                case SymmetricAlgorithmType.AesCbc:
                    return SymmetricAlgorithmNames.AesCbc;
                case SymmetricAlgorithmType.AesCbcPkcs7:
                    return SymmetricAlgorithmNames.AesCbcPkcs7;
                case SymmetricAlgorithmType.AesCcm:
                    return SymmetricAlgorithmNames.AesCcm;
                case SymmetricAlgorithmType.AesEcb:
                    return SymmetricAlgorithmNames.AesEcb;
                case SymmetricAlgorithmType.AesEcbPkcs7:
                    return SymmetricAlgorithmNames.AesEcbPkcs7;
                case SymmetricAlgorithmType.AesGcm:
                    return SymmetricAlgorithmNames.AesGcm;
                case SymmetricAlgorithmType.DesCbc:
                    return SymmetricAlgorithmNames.DesCbc;
                case SymmetricAlgorithmType.DesCbcPkcs7:
                    return SymmetricAlgorithmNames.DesCbcPkcs7;
                case SymmetricAlgorithmType.DesEcb:
                    return SymmetricAlgorithmNames.DesEcb;
                case SymmetricAlgorithmType.DesEcbPkcs7:
                    return SymmetricAlgorithmNames.DesEcbPkcs7;
                case SymmetricAlgorithmType.Rc2Cbc:
                    return SymmetricAlgorithmNames.Rc2Cbc;
                case SymmetricAlgorithmType.Rc2CbcPkcs7:
                    return SymmetricAlgorithmNames.Rc2CbcPkcs7;
                case SymmetricAlgorithmType.Rc2Ecb:
                    return SymmetricAlgorithmNames.Rc2Ecb;
                case SymmetricAlgorithmType.Rc2EcbPkcs7:
                    return SymmetricAlgorithmNames.Rc2EcbPkcs7;
                case SymmetricAlgorithmType.Rc4:
                    return SymmetricAlgorithmNames.Rc4;
                case SymmetricAlgorithmType.TripleDesCbc:
                    return SymmetricAlgorithmNames.TripleDesCbc;
                case SymmetricAlgorithmType.TripleDesCbcPkcs7:
                    return SymmetricAlgorithmNames.TripleDesCbcPkcs7;
                case SymmetricAlgorithmType.TripleDesEcb:
                    return SymmetricAlgorithmNames.TripleDesEcb;
                case SymmetricAlgorithmType.TripleDesEcbPkcs7:
                    return SymmetricAlgorithmNames.TripleDesEcbPkcs7;


                default:
                    throw new NotSupportedException();
            }
        }

        public static IJasilySymmetricKeyAlgorithmProvider CreateSymmetric(SymmetricAlgorithmType algorithmType)
            => new UAPSymmetricKeyAlgorithmProvider(
                    SymmetricKeyAlgorithmProvider.OpenAlgorithm(
                        ConvertToSymmetricKeyAlgorithmName(algorithmType)));
#else
        public static IJasilyAsymmetricKeyAlgorithmProvider CreateAsymmetric(AsymmetricAlgorithmType algorithmType)
        {
            throw new NotSupportedException();
        }

        public static IJasilySymmetricKeyAlgorithmProvider CreateSymmetric(SymmetricAlgorithmType algorithmType)
        {
            throw new NotSupportedException();
        }

        public static IJasilyHashAlgorithmProvider CreateHash(HashAlgorithmType hashAlgorithm)
        {
            throw new NotSupportedException();
        }
#endif
    }
}
