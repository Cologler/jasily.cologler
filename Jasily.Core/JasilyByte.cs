using System.IO;
using System.Text;

namespace System
{
    public static class JasilyByte
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

        public static string GetString(this byte[] bytes, Encoding encoding)
        {
            return encoding.GetString(bytes, 0, bytes.Length);
        }

        public static string GetString(this byte[] bytes)
        {
            return bytes.GetString(Encoding.UTF8);
        }
    }
}
