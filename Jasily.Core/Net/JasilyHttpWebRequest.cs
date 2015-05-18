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
                await request.GetResponseAsync();
                return Return();
            }
            catch (WebException e)
            {
                return Return(e);
            }
        }

        public static async Task<WebResult<T>> GetResultAsync<T>(this HttpWebRequest request, Func<Stream, T> selector)
        {
            try
            {
                using (var stream = (await request.GetResponseAsync()).GetResponseStream())
                {
                    return Return<T>(selector(stream));
                }
            }
            catch (WebException e)
            {
                return Return<T>(e);
            }
        }

        public static async Task<WebResult<T>> GetResultAsJsonAsync<T>(this HttpWebRequest request)
        {
            return await request.GetResultAsync(AsJson<T>);
        }

        public static async Task<WebResult<T>> GetResultAsXmlAsync<T>(this HttpWebRequest request)
        {
            return await request.GetResultAsync(AsXml<T>);
        }

        public static async Task<WebResult> SendAndGetResultAsync(this HttpWebRequest request, Stream input)
        {
            try
            {
                await request.SendAsync(input);
                return await request.GetResultAsync();
            }
            catch (WebException e)
            {
                return Return(e);
            }
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
                return Return<T>(e);
            }
        }

        public static async Task<WebResult<string>> SendAndGetResultAsTextAsync(this HttpWebRequest request, Stream input)
        {
            return await request.SendAndGetResultAsync(input, AsText);
        }

        public static async Task<WebResult<T>> SendAndGetResultAsJsonAsync<T>(this HttpWebRequest request, Stream input)
        {
            return await request.SendAndGetResultAsync(input, AsJson<T>);
        }

        public static async Task<WebResult<T>> SendAndGetResultAsXmlAsync<T>(this HttpWebRequest request, Stream input)
        {
            return await request.SendAndGetResultAsync(input, AsXml<T>);
        }

        private static WebResult Return()
        {
            return new WebResult();
        }

        private static WebResult Return(WebException e)
        {
            return new WebResult(e);
        }

        private static WebResult<T> Return<T>(T r)
        {
            return new WebResult<T>(r);
        }

        private static WebResult<T> Return<T>(WebException e)
        {
            return new WebResult<T>(e);
        }

        private static T AsJson<T>(Stream input)
        {
            return input.JsonToObject<T>();
        }

        private static T AsXml<T>(Stream input)
        {
            return input.XmlToObject<T>();
        }

        private static string AsText(Stream input)
        {
            return input.ToArray().GetString();
        }
    }
}
