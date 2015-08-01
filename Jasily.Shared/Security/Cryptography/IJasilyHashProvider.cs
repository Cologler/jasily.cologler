namespace Jasily.Security.Cryptography
{
    public interface IJasilyHashProvider
    {
        string ComputeHashString(byte[] text);
    }
}