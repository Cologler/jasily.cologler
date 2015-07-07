namespace System.Security.Cryptography
{
    public static class JasilyCryptoHelper
    {
        public static byte[] EncryptUseAes(this byte[] bytes, byte[] key, byte[] iv,
            CipherMode mode = CipherMode.CBC,
            PaddingMode padding = PaddingMode.PKCS7)
        {
            using (var aes = new AesManaged()
            {
                IV = iv,
                Key = key,
                Mode = mode,
                Padding = padding
            })
            {
                return aes.Encrypt(bytes);
            }
        }

        public static byte[] DecryptUseAes(this byte[] bytes, byte[] key, byte[] iv,
            CipherMode mode = CipherMode.CBC,
            PaddingMode padding = PaddingMode.PKCS7)
        {
            using (var aes = new AesManaged()
            {
                IV = iv,
                Key = key,
                Mode = mode,
                Padding = padding
            })
            {
                return aes.Decrypt(bytes);
            }
        }
    }
}