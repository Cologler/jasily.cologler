using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace System
{
    public static class ByteArrayExtensions
    {
        /// <summary>
        /// to readonly MemoryStream
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static MemoryStream ToMemoryStream(this byte[] bytes) => new MemoryStream(bytes, false);

        /// <summary>
        /// get string use special encoding
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string GetString(this byte[] bytes, Encoding encoding) => encoding.GetString(bytes, 0, bytes.Length);

        /// <summary>
        /// get string use encoding-utf8
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string GetString(this byte[] bytes) => bytes.GetString(Encoding.UTF8);

        public static string GetHexString(this IEnumerable<byte> bytes) => BitConverter.ToString(bytes.ToArray());

        public static string GetHexString(this byte[] bytes) => BitConverter.ToString(bytes);

        public static string GetHexString(this byte[] bytes, int startIndex) => BitConverter.ToString(bytes, startIndex);

        public static string GetHexString(this byte[] bytes, int startIndex, int length)
            => BitConverter.ToString(bytes, startIndex, length);

        public static void F(byte[] bytes, string template)
        {
            if (string.IsNullOrEmpty(template)) throw new ArgumentException("Argument is null or empty", nameof(template));
        }
    }
}
