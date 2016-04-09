using JetBrains.Annotations;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;

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
            return (await webResult).Cast(ByteArrayExtensions.GetString);
        }

        public static async Task<WebResult<T>> AsXml<T>([NotNull] this Task<WebResult<byte[]>> webResult)
        {
            if (webResult == null) throw new ArgumentNullException(nameof(webResult));
            return (await webResult).Cast(XmlSerializerExtensions.XmlToObject<T>);
        }

        public static async Task<WebResult<T>> AsJson<T>([NotNull] this Task<WebResult<byte[]>> webResult)
        {
            if (webResult == null) throw new ArgumentNullException(nameof(webResult));
            return (await webResult).Cast(DataContractJsonSerializerExtensions.JsonToObject<T>);
        }

        public static async Task<WebResult<T>> TryAsJson<T>([NotNull] this Task<WebResult<byte[]>> webResult) where T : class
        {
            if (webResult == null) throw new ArgumentNullException(nameof(webResult));
            return (await webResult).Cast(DataContractJsonSerializerExtensions.TryJsonToObject<T>);
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
                var response = await request.GetResponseAsync();
                using (var stream = response.GetResponseStream())
                {
                    return new WebResult<T>(response, selector(stream));
                }
            }
            catch (WebException e)
            {
                return new WebResult<T>(e);
            }
        }

        public static async Task<WebResult<T>> GetResultAsync<T>([NotNull] this HttpWebRequest request,
            [NotNull] Func<Stream, CancellationToken, T> selector, CancellationToken cancellationToken)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            try
            {
                var response = await request.GetResponseAsync(cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                using (var stream = response.GetResponseStream())
                {
                    return new WebResult<T>(response, selector(stream, cancellationToken));
                }
            }
            catch (WebException e)
            {
                return new WebResult<T>(e);
            }
        }

        #endregion

        #region GetResultAsBytesAsync

        public static async Task<WebResult<byte[]>> GetResultAsBytesAsync([NotNull] this HttpWebRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            try
            {
                return await request.GetResultAsync(AsBytes);
            }
            catch (IOException e)
            {
                if (Debugger.IsAttached) Debugger.Break();
                throw;
            }
        }

        public static async Task<WebResult<byte[]>> GetResultAsBytesAsync([NotNull] this HttpWebRequest request,
            CancellationToken cancellationToken)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            try
            {
                return await request.GetResultAsync(AsBytes, cancellationToken);
            }
            catch (IOException e)
            {
                if (Debugger.IsAttached) Debugger.Break();
                throw;
            }
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

            return await request.GetResultAsync();
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
                return await request.GetResultAsync<T>(selector);
            }
            catch (WebException e)
            {
                return new WebResult<T>(e);
            }
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
                return await request.GetResultAsync<T>(selector);
            }
            catch (WebException e)
            {
                return new WebResult<T>(e);
            }
        }

        #endregion

        #region SendAndGetResultAsBytesAsync

        public static async Task<WebResult<byte[]>> SendAndGetResultAsBytesAsync([NotNull] this HttpWebRequest request,
            [NotNull] Stream input)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            if (input == null) throw new ArgumentNullException(nameof(input));
            try
            {
                return await request.SendAndGetResultAsync(input, AsBytes);
            }
            catch (IOException e)
            {
                if (Debugger.IsAttached) Debugger.Break();
                throw;
            }
        }

        public static async Task<WebResult<byte[]>> SendAndGetResultAsBytesAsync([NotNull] this HttpWebRequest request,
            [NotNull] Stream input, CancellationToken cancellationToken)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            if (input == null) throw new ArgumentNullException(nameof(input));
            try
            {
                return await request.SendAndGetResultAsync(input, AsBytes, cancellationToken);
            }
            catch (IOException e)
            {
                if (Debugger.IsAttached) Debugger.Break();
                throw;
            }
        }

        #endregion

        #endregion

        #region loop request

        private static async Task<TWebResult> TryLoop<TWebResult>([NotNull] HttpWebRequest request, int tryTime,
            Func<HttpWebRequest, Task<TWebResult>> func)
            where TWebResult : WebResult
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            if (tryTime <= 0) throw new ArgumentOutOfRangeException(nameof(tryTime), "must > 0");
            Debug.Assert(func != null);

            TWebResult r;
            do
            {
                r = await func(request);
                if (r.IsSuccess) return r;
            } while (--tryTime > 0);
            return r;
        }

        private static async Task<TWebResult> TryLoop<TWebResult>([NotNull] HttpWebRequest request, int tryTime,
            Func<HttpWebRequest, CancellationToken, Task<TWebResult>> func, CancellationToken token)
            where TWebResult : WebResult
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            if (tryTime <= 0) throw new ArgumentOutOfRangeException(nameof(tryTime), "must > 0");
            Debug.Assert(func != null);

            TWebResult r;
            do
            {
                r = await func(request, token);
                if (r.IsSuccess) return r;
            } while (--tryTime > 0);
            return r;
        }

        public static async Task<WebResult> GetResultAsync(this HttpWebRequest request, int tryTime)
            => await TryLoop(request, tryTime, GetResultAsync);

        public static async Task<WebResult<T>> GetResultAsync<T>(this HttpWebRequest request, Func<Stream, T> selector,
            int tryTime) => await TryLoop(request, tryTime, z => z.GetResultAsync(selector));

        public static async Task<WebResult<byte[]>> GetResultAsBytesAsync(this HttpWebRequest request, int tryTime)
            => await TryLoop(request, tryTime, GetResultAsBytesAsync);

        #endregion
    }
}