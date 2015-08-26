using Windows.Security.Cryptography.Core;

namespace Jasily.Security.Cryptography
{
    internal sealed class UAPSymmetricKeyAlgorithmProvider : UAPEncryptAlgorithmProvider,
        IJasilySymmetricKeyAlgorithmProvider
    {
        private readonly SymmetricKeyAlgorithmProvider provider;
        private CryptographicKey key;

        public UAPSymmetricKeyAlgorithmProvider(SymmetricKeyAlgorithmProvider provider)
        {
            this.provider = provider;
        }

        public void CreateSymmetricKey()
            => this.key = this.provider.CreateSymmetricKey(null);
    }
}