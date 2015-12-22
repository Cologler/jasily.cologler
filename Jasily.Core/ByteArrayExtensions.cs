using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace System
{
    public static class ByteArrayExtensions
    {
        /// <summary>
        /// 从此数组派生出一个只读内存流
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static MemoryStream ToMemoryStream(this byte[] bytes)
        {
            return new MemoryStream(bytes, false);
        }

        /// <summary>
        /// get string use special encoding
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string GetString(this byte[] bytes, Encoding encoding)
        {
            return encoding.GetString(bytes, 0, bytes.Length);
        }

        /// <summary>
        /// get string use encoding-utf8
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string GetString(this byte[] bytes)
        {
            return bytes.GetString(Encoding.UTF8);
        }

        public static string GetHexString(this IEnumerable<byte> bytes)
        {
            return BitConverter.ToString(bytes.ToArray());
        }

        public static string GetHexString(this byte[] bytes)
        {
            return BitConverter.ToString(bytes);
        }

        public static string GetHexString(this byte[] bytes, int startIndex)
        {
            return BitConverter.ToString(bytes, startIndex);
        }

        public static string GetHexString(this byte[] bytes, int startIndex, int length)
        {
            return BitConverter.ToString(bytes, startIndex, length);
        }
    }
}
