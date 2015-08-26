using System;
using System.Threading.Tasks;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Jasily.Security.Cryptography.BaseOn;

namespace Jasily.Security.Cryptography
{
    internal abstract class UAPEncryptAlgorithmProvider : JasilyEncryptAlgorithmProvider
    {
        protected CryptographicKey Key { get; set; }

        public override bool HasKey => this.Key != null;

        public override byte[] Encrypt(byte[] content)
        {
            if (!this.HasKey) throw new InvalidOperationException();

            var buffer = CryptographicEngine.Encrypt(this.Key, content.ToCryptographicBuffer(), null);
            return buffer.CryptographicBufferToByteArray();
        }

        public override byte[] Decrypt(byte[] content)
        {
            if (!this.HasKey) throw new InvalidOperationException();

            var buffer = CryptographicEngine.Decrypt(this.Key, content.ToCryptographicBuffer(), null);
            return buffer.CryptographicBufferToByteArray();
        }

        public override async Task<byte[]> DecryptAsync(byte[] content)
        {
            if (!this.HasKey) throw new InvalidOperationException();

            var buffer = await CryptographicEngine.DecryptAsync(this.Key, content.ToCryptographicBuffer(), null);
            return buffer.CryptographicBufferToByteArray();
        }
    }
}