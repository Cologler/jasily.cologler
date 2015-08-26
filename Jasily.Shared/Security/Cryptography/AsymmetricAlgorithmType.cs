namespace Jasily.Security.Cryptography
{
    /// <summary>
    /// 不对称算法类型
    /// </summary>
    public enum AsymmetricAlgorithmType
    {
        DsaSha1,
        DsaSha256,
        EcdsaP256Sha256,
        EcdsaP384Sha384,
        EcdsaP521Sha512,
        EcdsaSha256,
        EcdsaSha384,
        EcdsaSha512,
        RsaOaepSha1,
        RsaOaepSha256,
        RsaOaepSha384,
        RsaOaepSha512,
        RsaPkcs1,
        RsaSignPkcs1Sha1,
        RsaSignPkcs1Sha256,
        RsaSignPkcs1Sha384,
        RsaSignPkcs1Sha512,
        RsaSignPssSha1,
        RsaSignPssSha256,
        RsaSignPssSha384,
        RsaSignPssSha512
    }
}