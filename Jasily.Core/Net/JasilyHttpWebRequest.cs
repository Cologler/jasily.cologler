using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Runtime.Serialization.Json;
using System.Xml.Serialization;

namespace System.Net
{
    public static class JasilyHttpWebRequest
    {
        /// <summary>
        /// 异常信息与 request 的 BeginGetRequestStream, EndGetRequestStream 相同。
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static async Task<Stream> GetRequestStreamAsync(this HttpWebRequest request)
        {
            var task = new TaskCompletionSource<Stream>();

            request.BeginGetRequestStream(ac =>
            {
                try
                {
                    task.SetResult(request.EndGetRequestStream(ac));
                }
                catch (Exception e)
                {
                    task.SetException(e);
                }
            }, null);

            return await task.Task;
        }

        /// <summary>
        /// 异常信息与 request 的 BeginGetResponse, EndGetResponse 相同。
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static async Task<WebResponse> GetResponseAsync(this HttpWebRequest request)
        {
            var task = new TaskCompletionSource<WebResponse>();

            request.BeginGetResponse(ac =>
            {
                try
                {
                    task.SetResult(request.EndGetResponse(ac));
                }
                catch (Exception e)
                {
                    task.SetException(e);
                }
            }, null);

            return await task.Task;
        }

        public static async Task SendAsync(this HttpWebRequest request, Stream input)
        {
            using (var stream = await request.GetRequestStreamAsync())
            {
                await input.CopyToAsync(stream);
            }
        }

        public static async Task<WebResult> GetResultAsync(this HttpWebRequest request)
        {
            try
            {
                return new WebResult(await request.GetResponseAsync());
            }
            catch (WebException e)
            {
                return new WebResult(e);
            }
        }

        public static async Task<WebResult<T>> GetResultAsync<T>(this HttpWebRequest request, Func<Stream, T> selector)
        {
            WebResponse response = null;

            try
            {
                response = await request.GetResponseAsync();
                using (var stream = response.GetResponseStream())
                {
                    return new WebResult<T>(response, selector(stream));
                }
            }
            catch (WebException e)
            {
                return new WebResult<T>(response, e);
            }
        }

        public static async Task<WebResult<byte[]>> GetResultAsBytesAsync(this HttpWebRequest request)
        {
            return await request.GetResultAsync(AsBytes);
        }

        public static async Task<WebResult> SendAndGetResultAsync(this HttpWebRequest request, Stream input)
        {
            try
            {
                await request.SendAsync(input);
            }
            catch (WebException e)
            {
                return new WebResult(e);
            }

            return await request.GetResultAsync();
        }

        public static async Task<WebResult<byte[]>> SendAndGetResultAsBytesAsync(this HttpWebRequest request, Stream input)
        {
            return await request.SendAndGetResultAsync(input, AsBytes);
        }

        public static async Task<WebResult<T>> SendAndGetResultAsync<T>(this HttpWebRequest request, Stream input, Func<Stream, T> selector)
        {
            try
            {
                await request.SendAsync(input);
                return await request.GetResultAsync<T>(selector);
            }
            catch (WebException e)
            {
                return new WebResult<T>(e);
            }
        }

        private static byte[] AsBytes(Stream input)
        {
            return input.ToArray();
        }
    }
}
