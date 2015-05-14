using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace System.Xml.Serialization
{
    public static class JasilyXmlSerializer
    {
        public static T XmlToObject<T>(this System.IO.Stream stream)
        {
            try
            {
#if DEBUG
                var bytes = stream.ToArray();
                Debug.WriteLineIf(true, bytes.GetString());
                stream = bytes.ToMemoryStream();
#endif
                var serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
                var obj = (T)serializer.Deserialize(stream);
                return obj;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static T XmlToObject<T>(this byte[] bytes)
        {
            using (var ms = new System.IO.MemoryStream(bytes))
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
                var serializer = new System.Xml.Serialization.XmlSerializer(obj.GetType());
                serializer.Serialize(ms, obj);
                var xml = ms.ToArray();
                return Encoding.UTF8.GetString(xml, 0, xml.Length);
            }
        }
    }
}
