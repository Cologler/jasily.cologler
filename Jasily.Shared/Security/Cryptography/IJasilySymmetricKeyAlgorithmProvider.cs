namespace Jasily.Security.Cryptography
{
    public interface IJasilySymmetricKeyAlgorithmProvider : IJasilyEncryptAlgorithmProvider
    {
        void CreateSymmetricKey();
    }
}