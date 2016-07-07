using System.Diagnostics;
using System.IO;
using System.Text;
using JetBrains.Annotations;

namespace System.Runtime.Serialization.Json
{
    public static class DataContractJsonSerializerExtensions
    {
        public static DataContractJsonSerializerSettings SerializerSettings { get; set; }

        private static DataContractJsonSerializer GetSerializer<T>()
        {
            return SerializerSettings == null
                ? new DataContractJsonSerializer(typeof(T))
                : new DataContractJsonSerializer(typeof(T), SerializerSettings);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stream"></param>
        /// <exception cref="System.Runtime.Serialization.SerializationException"></exception>
        /// <returns></returns>
        public static T JsonToObject<T>([NotNull] this Stream stream)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));

#if DEBUG
            var bytes = stream.ToArray();
            stream = bytes.ToMemoryStream();
#endif
            try
            {
                var ser = GetSerializer<T>();
                var obj = (T)ser.ReadObject(stream);
                return obj;
            }
            catch (Exception)
            {
#if DEBUG
                Debug.WriteLine("JsonToObject() failed: {0}", bytes.GetString());
#endif
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="bytes"></param>
        /// <exception cref="System.Runtime.Serialization.SerializationException"></exception>
        /// <returns></returns>
        public static T JsonToObject<T>([NotNull] this byte[] bytes)
        {
            using (var ms = new MemoryStream(bytes))
            {
                return ms.JsonToObject<T>();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonDoc"></param>
        /// <param name="encoding"></param>
        /// <exception cref="System.Runtime.Serialization.SerializationException"></exception>
        /// <returns></returns>
        public static T JsonToObject<T>([NotNull] this string jsonDoc, [NotNull] Encoding encoding)
        {
            if (jsonDoc == null) throw new ArgumentNullException(nameof(jsonDoc));
            if (encoding == null) throw new ArgumentNullException(nameof(encoding));

            return encoding.GetBytes(jsonDoc).JsonToObject<T>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonDoc"></param>
        /// <exception cref="System.Runtime.Serialization.SerializationException"></exception>
        /// <returns></returns>
        public static T JsonToObject<T>([NotNull] this string jsonDoc)
            => jsonDoc.JsonToObject<T>(Encoding.UTF8);

        public static T TryJsonToObject<T>([NotNull] this Stream stream) where T : class
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            try
            {
                return stream.JsonToObject<T>();
            }
            catch (SerializationException)
            {
                return null;
            }
        }

        public static T TryJsonToObject<T>([NotNull] this byte[] bytes) where T : class
        {
            if (bytes == null) throw new ArgumentNullException(nameof(bytes));
            try
            {
                return bytes.JsonToObject<T>();
            }
            catch (SerializationException)
            {
                return null;
            }
        }

        public static T TryJsonToObject<T>([NotNull] this string jsonDoc, [NotNull] Encoding encoding) where T : class
        {
            if (jsonDoc == null) throw new ArgumentNullException(nameof(jsonDoc));
            if (encoding == null) throw new ArgumentNullException(nameof(encoding));

            try
            {
                return jsonDoc.JsonToObject<T>(encoding);
            }
            catch (SerializationException)
            {
                return null;
            }
        }

        public static T TryJsonToObject<T>([NotNull] this string jsonDoc) where T : class
        {
            if (jsonDoc == null) throw new ArgumentNullException(nameof(jsonDoc));
            try
            {
                return jsonDoc.JsonToObject<T>();
            }
            catch (SerializationException)
            {
                return null;
            }
        }

        public static string ObjectToJson([NotNull] this object obj)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));

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
