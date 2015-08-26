namespace Jasily.Security.Cryptography
{
    public interface IJasilyAsymmetricKeyAlgorithmProvider : IJasilyEncryptAlgorithmProvider
    {
        void CreateKey(uint keySize);
        void ImportKeyPair(byte[] key);
        void ImportPublicKey(byte[] key);
    }
}