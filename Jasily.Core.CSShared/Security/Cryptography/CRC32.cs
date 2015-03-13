
namespace System.Security.Cryptography
{
    public abstract class CRC32 : HashAlgorithm
    {
        public static CRC32 Create()
        {
            return new CRC32CryptoServiceProvider();
        }
    }
}
