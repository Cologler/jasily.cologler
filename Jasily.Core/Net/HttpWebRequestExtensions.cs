using System.IO;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace System.Net
{
    public static class HttpWebRequestExtensions
    {
        /// <summary>
        /// 异常信息与 request 的 BeginGetRequestStream, EndGetRequestStream 相同。
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static Task<Stream> GetRequestStreamAsync([NotNull] this HttpWebRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            return Task<Stream>.Factory.FromAsync(
                request.BeginGetRequestStream,
                request.EndGetRequestStream, null);
        }

        public static Task<Stream> GetRequestStreamAsync([NotNull] this HttpWebRequest request,
            CancellationToken cancellationToken)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            return Task<Stream>.Factory.FromAsync(
                request.BeginGetRequestStream,
                request.EndGetRequestStream,
                cancellationToken, request.Abort);
        }

        /// <summary>
        /// 异常信息与 request 的 BeginGetResponse, EndGetResponse 相同。
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static Task<WebResponse> GetResponseAsync([NotNull] this HttpWebRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            return Task<WebResponse>.Factory.FromAsync(
                request.BeginGetResponse,
                request.EndGetResponse, null);
        }

        public static Task<WebResponse> GetResponseAsync([NotNull] this HttpWebRequest request,
            CancellationToken cancellationToken)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            return Task<WebResponse>.Factory.FromAsync(
                request.BeginGetResponse,
                request.EndGetResponse,
                cancellationToken, request.Abort);
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

            using (var stream = await request.GetRequestStreamAsync(cancellationToken))
            {
                await input.CopyToAsync(stream, cancellationToken);
            }
        }
    }
}
