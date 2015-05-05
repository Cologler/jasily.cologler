
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
    }
}
