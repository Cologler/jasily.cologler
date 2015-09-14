using Windows.Storage.Streams;

namespace Windows.Security.Cryptography
{
    public static class IbufferExtensions
    {
        public static byte[] CryptographicBufferToByteArray(this IBuffer buffer)
        {
            byte[] result;
            CryptographicBuffer.CopyToByteArray(buffer, out result);
            return result;
        }

        public static IBuffer ToCryptographicBuffer(this byte[] buffer)
            => CryptographicBuffer.CreateFromByteArray(buffer);
    }
}