using System.Threading.Tasks;

namespace Jasily.Security.Cryptography.BaseOn
{
    public abstract class JasilyEncryptAlgorithmProvider : IJasilyEncryptAlgorithmProvider
    {
        public abstract bool HasKey { get; }

        public abstract byte[] Encrypt(byte[] content);

        public abstract byte[] Decrypt(byte[] content);

        public async virtual Task<byte[]> EncryptAsync(byte[] content)
            => await Task.Run(() => this.Encrypt(content));

        public async virtual Task<byte[]> DecryptAsync(byte[] content)
            => await Task.Run(() => this.Decrypt(content));
    }
}