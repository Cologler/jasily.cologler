using System.Runtime.Serialization.Json;
using System.Xml.Serialization;

namespace System.Net
{
    public static class WebResultExtensions
    {
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
    }
}