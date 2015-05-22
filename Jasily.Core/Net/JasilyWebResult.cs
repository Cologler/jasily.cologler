using System.IO;
using System.Runtime.Serialization.Json;
using System.Xml.Serialization;

namespace System.Net
{
    public static class JasilyWebResult
    {
        public static WebResult<string> AsText(this WebResult<byte[]> webResult)
        {
            try
            {
                return new WebResult<string>(webResult.GetResultOrThrow().GetString());
            }
            catch (WebException e)
            {
                return new WebResult<string>(e);
            }
        }

        public static WebResult<T> AsXml<T>(this WebResult<byte[]> webResult)
        {
            try
            {
                return new WebResult<T>(webResult.GetResultOrThrow().XmlToObject<T>());
            }
            catch (WebException e)
            {
                return new WebResult<T>(e);
            }
        }

        public static WebResult<T> AsJson<T>(this WebResult<byte[]> webResult)
        {
            try
            {
                return new WebResult<T>(webResult.GetResultOrThrow().JsonToObject<T>());
            }
            catch (WebException e)
            {
                return new WebResult<T>(e);
            }
        }

        public static WebResult<TOut> ConvertTo<TIn, TOut>(this WebResult<TIn> webResult, Func<TIn, TOut> selector)
        {
            try
            {
                return new WebResult<TOut>(selector(webResult.GetResultOrThrow()));
            }
            catch (WebException e)
            {
                return new WebResult<TOut>(e);
            }
        }
    }
}