using System.IO;
using System.Text;

namespace System.Runtime.Serialization.Json
{
    public static class DataContractJsonSerializerExtensions 
    {
        public static T JsonToObject<T>(this Stream stream)
        {
            var ser = new DataContractJsonSerializer(typeof(T));
            var obj = (T)ser.ReadObject(stream);
            return obj;
        }

        public static T JsonToObject<T>(this byte[] bytes)
        {
            using (var ms = new MemoryStream(bytes))
            {
                return ms.JsonToObject<T>();
            }
        }

        public static T JsonToObject<T>(this string jsonDoc, Encoding encoding)
        {
            return encoding.GetBytes(jsonDoc).JsonToObject<T>();
        }

        public static T JsonToObject<T>(this string jsonDoc)
        {
            return jsonDoc.JsonToObject<T>(Encoding.UTF8);
        }

        public static string ObjectToJson(this object obj)
        {
            using (var ms = new MemoryStream())
            {
                var ser = new DataContractJsonSerializer(obj.GetType());
                ser.WriteObject(ms, obj);
                var json = ms.ToArray();
                return Encoding.UTF8.GetString(json, 0, json.Length);
            }
        }
    }
}
