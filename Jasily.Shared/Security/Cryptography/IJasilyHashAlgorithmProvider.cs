namespace Jasily.Security.Cryptography
{
    public interface IJasilyHashAlgorithmProvider
    {
        string ComputeHashString(byte[] text);
    }
}