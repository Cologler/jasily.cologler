
namespace System.Security.Cryptography
{
    // ReSharper disable once InconsistentNaming
    public abstract class CRC32 : HashAlgorithm
    {
        public new static CRC32 Create()
        {
            return new CRC32CryptoServiceProvider();
        }
    }
}
