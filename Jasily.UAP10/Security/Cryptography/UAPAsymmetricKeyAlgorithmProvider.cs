using System.Security.Authentication;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.UI.Notifications;

namespace Jasily.Security.Cryptography
{
    internal sealed class UAPAsymmetricKeyAlgorithmProvider : UAPEncryptAlgorithmProvider,
        IJasilyAsymmetricKeyAlgorithmProvider
    {
        private readonly AsymmetricKeyAlgorithmProvider provider;

        public UAPAsymmetricKeyAlgorithmProvider(AsymmetricKeyAlgorithmProvider provider)
        {
            this.provider = provider;
        }

        public void CreateKey(uint keySize)
            => this.Key = this.provider.CreateKeyPair(keySize);

        public void ImportKeyPair(byte[] key)
            => this.Key = this.provider.ImportKeyPair(CryptographicBuffer.CreateFromByteArray(key));

        public void ImportPublicKey(byte[] key)
            => this.Key = this.provider.ImportPublicKey(CryptographicBuffer.CreateFromByteArray(key));
    }
}