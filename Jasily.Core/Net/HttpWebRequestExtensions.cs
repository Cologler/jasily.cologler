using JetBrains.Annotations;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net
{
    public static class HttpWebRequestExtensions
    {
        /// <summary>
        /// 异常信息与 request 的 BeginGetRequestStream, EndGetRequestStream 相同。
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static async Task<Stream> GetRequestStreamAsync([NotNull] this HttpWebRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

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
        public static async Task<WebResponse> GetResponseAsync([NotNull] this HttpWebRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

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

        public static async Task<WebResponse> GetResponseAsync([NotNull] this HttpWebRequest request,
            CancellationToken cancellationToken)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var task = new TaskCompletionSource<WebResponse>();
            cancellationToken.Register(() => task.TrySetCanceled());
            request.BeginGetResponse(ac =>
            {
                WebResponse response;
                try
                {
                    response = request.EndGetResponse(ac);
                }
                catch (Exception e)
                {
                    task.TrySetException(e);
                    return;
                }
                task.TrySetResult(response);
            }, null);
            return await task.Task;
        }

        public static async Task SendAsync([NotNull] this HttpWebRequest request, [NotNull] Stream input)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            if (input == null) throw new ArgumentNullException(nameof(input));

            using (var stream = await request.GetRequestStreamAsync())
            {
                await input.CopyToAsync(stream);
            }
        }

        public static async Task SendAsync([NotNull] this HttpWebRequest request, [NotNull] Stream input,
            CancellationToken cancellationToken)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            if (input == null) throw new ArgumentNullException(nameof(input));

            using (var stream = await request.GetRequestStreamAsync())
            {
                await input.CopyToAsync(stream, cancellationToken);
            }
        }
    }
}
