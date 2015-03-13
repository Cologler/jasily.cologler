using System.IO;
using System.Threading.Tasks;

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
    }
}
