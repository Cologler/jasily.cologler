using System.Threading.Tasks;

namespace Jasily.Security.Cryptography
{
    public interface IJasilyEncryptAlgorithmProvider
    {
        bool HasKey { get; }

        byte[] Decrypt(byte[] content);
        Task<byte[]> DecryptAsync(byte[] content);
        byte[] Encrypt(byte[] content);
        Task<byte[]> EncryptAsync(byte[] content);
    }
}