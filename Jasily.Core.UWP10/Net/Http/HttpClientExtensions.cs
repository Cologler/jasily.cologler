using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Windows.Net.Http
{
    public static class HttpClientExtensions
    {
        public static Task<byte[]> TryGetByteArrayAsync(this HttpClient client, Uri requestUri)
        {
            try
            {
                return client.GetByteArrayAsync(requestUri);
            }
            catch (HttpRequestException)
            {
                return null;
            }
        }
        public static Task<byte[]> TryGetByteArrayAsync(this HttpClient client, string requestUri)
        {
            try
            {
                return client.GetByteArrayAsync(requestUri);
            }
            catch (HttpRequestException)
            {
                return null;
            }
        }
    }
}