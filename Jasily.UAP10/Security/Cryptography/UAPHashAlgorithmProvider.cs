using System;
using System.Text;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;

namespace Jasily.Security.Cryptography
{
    internal sealed class UAPHashAlgorithmProvider : IJasilyHashProvider
    {
        private readonly HashAlgorithmProvider provider;

        public UAPHashAlgorithmProvider(HashAlgorithmProvider provider)
        {
            this.provider = provider;
        }

        public string ComputeHashString(byte[] text)
        {
            if (text == null) throw new ArgumentNullException(nameof(text));

            var buffer = CryptographicBuffer.CreateFromByteArray(text);
            var hashed = this.provider.HashData(buffer);
            return CryptographicBuffer.EncodeToHexString(hashed);
        }
    }
}