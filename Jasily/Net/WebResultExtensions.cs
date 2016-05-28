using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using JetBrains.Annotations;

namespace Jasily.Net
{
    public static class WebResultExtensions
    {
        #region convert

        public static WebResult<string> AsText([NotNull] this WebResult<byte[]> webResult)
        {
            if (webResult == null) throw new ArgumentNullException(nameof(webResult));
            return webResult.Cast(ByteArrayExtensions.GetString);
        }

        public static WebResult<string> AsText([NotNull] this WebResult<byte[]> webResult,
            [NotNull] Encoding encoding)
        {
            if (webResult == null) throw new ArgumentNullException(nameof(webResult));
            if (encoding == null) throw new ArgumentNullException(nameof(encoding));
            return webResult.Cast(z => z.GetString(encoding));
        }

        /// <summary>
        /// auto parse encoding, but take more time
        /// </summary>
        /// <param name="webResult"></param>
        /// <returns></returns>
        public static WebResult<string> AsPowerText([NotNull] this WebResult<byte[]> webResult)
        {
            if (webResult == null) throw new ArgumentNullException(nameof(webResult));
            return webResult.Cast(z =>
            {
                if (webResult.Response != null)
                {
                    var encoding = GetEncodingFromText(webResult.Response.ContentType);
                    if (encoding != null) return z.GetString(encoding);
                }
                var doc = z.GetString(Encoding.UTF8);
                var docArray = doc.AsLines();
                var metas = docArray.Where(x => x.StartsWith("<meta http-equiv=\"Content-Type\"")).ToArray();
                if (metas.Length > 0)
                {
                    var encoding = metas.Select(GetEncodingFromText).FirstOrDefault(x => x != null);
                    if (encoding != null) return z.GetString(encoding);
                }
                return doc;
            });
        }

        private static Encoding GetEncodingFromText(string text)
        {
            if (text.Contains("charset=gbk") || text.Contains("charset=gb2312"))
                return JasilyEncoding.GetEncoding("gbk");
            return null;
        }

        public static WebResult<T> AsXml<T>([NotNull] this WebResult<byte[]> webResult)
        {
            if (webResult == null) throw new ArgumentNullException(nameof(webResult));
            return webResult.Cast(XmlSerializerExtensions.XmlToObject<T>);
        }

        public static WebResult<T> AsJson<T>([NotNull] this WebResult<byte[]> webResult)
        {
            if (webResult == null) throw new ArgumentNullException(nameof(webResult));
            return webResult.Cast(DataContractJsonSerializerExtensions.JsonToObject<T>);
        }

        public static WebResult<T> TryAsJson<T>([NotNull] this WebResult<byte[]> webResult) where T : class
        {
            if (webResult == null) throw new ArgumentNullException(nameof(webResult));
            return webResult.Cast(DataContractJsonSerializerExtensions.TryJsonToObject<T>);
        }

        #region async

        public static async Task<WebResult<string>> AsText([NotNull] this Task<WebResult<byte[]>> webResult)
        {
            if (webResult == null) throw new ArgumentNullException(nameof(webResult));
            return (await webResult).AsText();
        }

        public static async Task<WebResult<string>> AsText([NotNull] this Task<WebResult<byte[]>> webResult,
            [NotNull] Encoding encoding)
        {
            if (webResult == null) throw new ArgumentNullException(nameof(webResult));
            return (await webResult).AsText(encoding);
        }

        public static async Task<WebResult<string>> AsPowerText([NotNull] this Task<WebResult<byte[]>> webResult)
        {
            if (webResult == null) throw new ArgumentNullException(nameof(webResult));
            return (await webResult).AsPowerText();
        }

        public static async Task<WebResult<T>> AsXml<T>([NotNull] this Task<WebResult<byte[]>> webResult)
        {
            if (webResult == null) throw new ArgumentNullException(nameof(webResult));
            return (await webResult).AsXml<T>();
        }

        public static async Task<WebResult<T>> AsJson<T>([NotNull] this Task<WebResult<byte[]>> webResult)
        {
            if (webResult == null) throw new ArgumentNullException(nameof(webResult));
            return (await webResult).AsJson<T>();
        }

        public static async Task<WebResult<T>> TryAsJson<T>([NotNull] this Task<WebResult<byte[]>> webResult) where T : class
        {
            if (webResult == null) throw new ArgumentNullException(nameof(webResult));
            return (await webResult).TryAsJson<T>();
        }

        #endregion

        private static byte[] AsBytes(Stream input)
            => input.ToArray();

        private static byte[] AsBytes(Stream input, CancellationToken cancellationToken)
            => input.ToArray(cancellationToken);

        #endregion

        #region base request

        #region GetResultAsync

        public static async Task<WebResult> GetResultAsync([NotNull] this HttpWebRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            try
            {
                return new WebResult(await request.GetResponseAsync());
            }
            catch (WebException e)
            {
                return new WebResult(e);
            }
        }

        public static async Task<WebResult> GetResultAsync([NotNull] this HttpWebRequest request,
            CancellationToken cancellationToken)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            try
            {
                return new WebResult(await request.GetResponseAsync(cancellationToken));
            }
            catch (WebException e)
            {
                return new WebResult(e);
            }
        }

        public static async Task<WebResult<T>> GetResultAsync<T>([NotNull] this HttpWebRequest request,
            [NotNull] Func<Stream, T> selector)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            try
            {
                return await Task.Run(async () =>
                {
                    var response = await request.GetResponseAsync();
                    using (var stream = response.GetResponseStream())
                    {
                        return new WebResult<T>(response, selector(stream));
                    }
                });
            }
            catch (WebException e)
            {
                return new WebResult<T>(e);
            }
            catch (IOException e) when (e.InnerException?.ToString() == "System.Net.Sockets.SocketException")
            {
                if (Debugger.IsAttached) Debugger.Break();
                return new WebResult<T>(new WebException(e.InnerException.Message, e));
            }
            catch (Exception e)
            {
                if (Debugger.IsAttached) Debugger.Break();
                throw;
            }
        }

        public static async Task<WebResult<T>> GetResultAsync<T>([NotNull] this HttpWebRequest request,
            [NotNull] Func<Stream, CancellationToken, T> selector, CancellationToken cancellationToken)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            try
            {
                return await Task.Run(async () =>
                {
                    var response = await request.GetResponseAsync(cancellationToken);
                    cancellationToken.ThrowIfCancellationRequested();
                    using (var stream = response.GetResponseStream())
                    {
                        return new WebResult<T>(response, selector(stream, cancellationToken));
                    }
                }, cancellationToken);
            }
            catch (WebException e)
            {
                return new WebResult<T>(e);
            }
            catch (IOException e) when (e.InnerException?.ToString() == "System.Net.Sockets.SocketException")
            {
                if (Debugger.IsAttached) Debugger.Break();
                return new WebResult<T>(new WebException(e.InnerException.Message, e));
            }
        }

        #endregion

        #region GetResultAsBytesAsync

        public static async Task<WebResult<byte[]>> GetResultAsBytesAsync([NotNull] this HttpWebRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            return await request.GetResultAsync(AsBytes);
        }

        public static async Task<WebResult<byte[]>> GetResultAsBytesAsync([NotNull] this HttpWebRequest request,
            CancellationToken cancellationToken)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            return await request.GetResultAsync(AsBytes, cancellationToken);
        }

        #endregion

        #region SendAndGetResultAsync

        public static async Task<WebResult> SendAndGetResultAsync([NotNull] this HttpWebRequest request,
            [NotNull] Stream input)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            if (input == null) throw new ArgumentNullException(nameof(input));
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

        public static async Task<WebResult> SendAndGetResultAsync([NotNull] this HttpWebRequest request,
            [NotNull] Stream input, CancellationToken cancellationToken)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            if (input == null) throw new ArgumentNullException(nameof(input));
            try
            {
                await request.SendAsync(input, cancellationToken);
            }
            catch (WebException e)
            {
                return new WebResult(e);
            }

            return await request.GetResultAsync(cancellationToken);
        }

        public static async Task<WebResult<T>> SendAndGetResultAsync<T>([NotNull] this HttpWebRequest request,
            [NotNull] Stream input, [NotNull] Func<Stream, T> selector)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            if (input == null) throw new ArgumentNullException(nameof(input));
            if (selector == null) throw new ArgumentNullException(nameof(selector));
            try
            {
                await request.SendAsync(input);
            }
            catch (WebException e)
            {
                return new WebResult<T>(e);
            }

            return await request.GetResultAsync(selector);
        }

        public static async Task<WebResult<T>> SendAndGetResultAsync<T>([NotNull] this HttpWebRequest request,
            [NotNull] Stream input, [NotNull] Func<Stream, T> selector, CancellationToken cancellationToken)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            if (input == null) throw new ArgumentNullException(nameof(input));
            if (selector == null) throw new ArgumentNullException(nameof(selector));
            try
            {
                await request.SendAsync(input, cancellationToken);
            }
            catch (WebException e)
            {
                return new WebResult<T>(e);
            }
            return await request.GetResultAsync<T>(selector);
        }

        #endregion

        #region SendAndGetResultAsBytesAsync

        public static async Task<WebResult<byte[]>> SendAndGetResultAsBytesAsync([NotNull] this HttpWebRequest request,
            [NotNull] Stream input)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            if (input == null) throw new ArgumentNullException(nameof(input));
            return await request.SendAndGetResultAsync(input, AsBytes);
        }

        public static async Task<WebResult<byte[]>> SendAndGetResultAsBytesAsync([NotNull] this HttpWebRequest request,
            [NotNull] Stream input, CancellationToken cancellationToken)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            if (input == null) throw new ArgumentNullException(nameof(input));
            return await request.SendAndGetResultAsync(input, AsBytes, cancellationToken);
        }

        #endregion

        #endregion

        #region loop request

        private static async Task<TWebResult> TryLoop<TWebResult>([NotNull] HttpWebRequest request, int retryTime,
            Func<HttpWebRequest, Task<TWebResult>> func)
            where TWebResult : WebResult
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            if (retryTime <= 0) throw new ArgumentOutOfRangeException(nameof(retryTime), "must > 0");
            Debug.Assert(func != null);

            TWebResult r;
            do
            {
                r = await func(request);
                if (r.IsSuccess) return r;
            } while (--retryTime > 0);
            return r;
        }

        private static async Task<TWebResult> TryLoop<TWebResult>([NotNull] HttpWebRequest request, int retryTime,
            Func<HttpWebRequest, CancellationToken, Task<TWebResult>> func, CancellationToken token)
            where TWebResult : WebResult
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            if (retryTime <= 0) throw new ArgumentOutOfRangeException(nameof(retryTime), "must > 0");
            Debug.Assert(func != null);

            TWebResult r;
            do
            {
                r = await func(request, token);
                if (r.IsSuccess) return r;
            } while (--retryTime > 0);
            return r;
        }

        public static async Task<WebResult> GetResultAsync(this HttpWebRequest request, int retryTime)
            => await TryLoop(request, retryTime, GetResultAsync);

        public static async Task<WebResult<T>> GetResultAsync<T>(this HttpWebRequest request, Func<Stream, T> selector,
            int retryTime) => await TryLoop(request, retryTime, z => z.GetResultAsync(selector));

        public static async Task<WebResult<byte[]>> GetResultAsBytesAsync(this HttpWebRequest request, int retryTime)
            => await TryLoop(request, retryTime, GetResultAsBytesAsync);

        #endregion
    }
}