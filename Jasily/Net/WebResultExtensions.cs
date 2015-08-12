using System;
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

        public static WebResult<string> AsText(this WebResult<byte[]> webResult)
        {
            return webResult.Cast(ByteArrayExtensions.GetString);
        }

        public static WebResult<T> AsXml<T>(this WebResult<byte[]> webResult)
        {
            return webResult.Cast(XmlSerializerExtensions.XmlToObject<T>);
        }

        public static WebResult<T> AsJson<T>(this WebResult<byte[]> webResult)
        {
            return webResult.Cast(DataContractJsonSerializerExtensions.JsonToObject<T>);
        }

        #endregion

        #region request

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

        public static async Task<WebResult> SendAndGetResultAsync(this HttpWebRequest request, Stream input, CancellationToken cancellationToken)
        {
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

        public static async Task<WebResult<byte[]>> SendAndGetResultAsBytesAsync(this HttpWebRequest request, Stream input)
        {
            return await request.SendAndGetResultAsync(input, AsBytes);
        }

        public static async Task<WebResult<byte[]>> SendAndGetResultAsBytesAsync(this HttpWebRequest request, Stream input, CancellationToken cancellationToken)
        {
            return await request.SendAndGetResultAsync(input, AsBytes, cancellationToken);
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

        public static async Task<WebResult<T>> SendAndGetResultAsync<T>(this HttpWebRequest request, Stream input, Func<Stream, T> selector, CancellationToken cancellationToken)
        {
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

        private static byte[] AsBytes(Stream input)
        {
            return input.ToArray();
        }
    }
}