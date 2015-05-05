using System.Text;

namespace System.Xml.Serialization
{
    public static class JasilyXmlSerializer
    {
        public static T XmlToObject<T>(this System.IO.Stream stream)
        {
            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
            T obj = (T)serializer.Deserialize(stream);
            return obj;
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
    }
}
