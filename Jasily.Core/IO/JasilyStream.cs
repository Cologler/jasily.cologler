using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.IO
{
    public static class JasilyStream
    {
        /// <summary>
        /// 将流内容写入字节数组，而与 System.IO.Stream.Position 属性无关。
        /// </summary>
        /// <param name="stream"></param>
        /// <returns>新字节数组。</returns>
        public static byte[] ToArray(this Stream stream)
        {
            using (var ms = new MemoryStream())
            {
                var pos = stream.Position;
                stream.Position = 0;
                stream.CopyTo(ms);
                stream.Position = pos;
                return ms.ToArray();
            }
        }
    }
}
