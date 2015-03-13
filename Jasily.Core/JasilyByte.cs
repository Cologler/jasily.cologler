using System.IO;

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
    }
}
