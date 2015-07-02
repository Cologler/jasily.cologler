using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace System.Xml.Serialization
{
    public static class XmlSerializerExtensions
    {
        public static T XmlToObject<T>(this Stream stream)
        {
#if DEBUG
            var bytes = stream.ToArray();
            stream = bytes.ToMemoryStream();
#endif

            try
            {
                var serializer = new XmlSerializer(typeof(T));
                var obj = (T)serializer.Deserialize(stream);
                return obj;
            }
            catch (Exception)
            {
#if DEBUG
                Debug.WriteLine("XmlToObject() failed: {0}", bytes.GetString());
#endif
                throw;
            }
        }

        public static T XmlToObject<T>(this byte[] bytes)
        {
            using (var ms = new MemoryStream(bytes))
            {
                return ms.XmlToObject<T>();
            }
        }

        public static T XmlToObject<T>(this string xmlDoc, Encoding encoding)
        {
            return encoding.GetBytes(xmlDoc).XmlToObject<T>();
        }

        public static T XmlToObject<T>(this string xmlDoc)
        {
            return xmlDoc.XmlToObject<T>(Encoding.UTF8);
        }

        public static string ObjectToXml(this object obj)
        {
            using (var ms = new MemoryStream())
            {
                var serializer = new XmlSerializer(obj.GetType());
                serializer.Serialize(ms, obj);
                return ms.ToArray().GetString();
            }
        }
    }
}
