
using System.Threading.Tasks;

namespace System.IO
{
    public static class JasilyStream
    {
        /// <summary>
        /// 将流内容写入字节数组。
        /// </summary>
        /// <param name="stream"></param>
        /// <returns>新字节数组。</returns>
        public static byte[] ToArray(this Stream stream)
        {
            using (var ms = new MemoryStream())
            {
                if (stream.CanSeek)
                {
                    stream.Position = 0;
                }

                stream.CopyTo(ms);
                return ms.ToArray();
            }
        }

        /// <summary>
        /// write whole buffer into stream
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="buffer"></param>
        public static void Write(this Stream stream, byte[] buffer)
        {
            stream.Write(buffer, 0, buffer.Length);
        }

        /// <summary>
        /// write whole buffer into stream
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static async Task WriteAsync(this Stream stream, byte[] buffer)
        {
            await stream.WriteAsync(buffer, 0, buffer.Length);
        }
    }
}
