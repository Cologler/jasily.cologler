using System.Threading;
using System.Threading.Tasks;

namespace System.IO
{
    public static class StreamExtensions
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

        #region copy

        /// <summary>
        /// see http://referencesource.microsoft.com/#mscorlib/system/io/stream.cs,2a0f078c2e0c0aa8
        /// </summary>
        private const int DefaultCopyBufferSize = 81920;

        public static void CopyTo(this Stream stream, Stream destination, IObserver<long> progressChangedWatcher)
        {
            CopyTo(stream, destination, DefaultCopyBufferSize, progressChangedWatcher);
        }

        public static void CopyTo(this Stream stream, Stream destination, int bufferSize, IObserver<long> progressChangedWatcher)
        {
            if (progressChangedWatcher == null) throw new ArgumentNullException(nameof(progressChangedWatcher));

            long total = 0;
            byte[] buffer = new byte[bufferSize];
            int read;
            while ((read = stream.Read(buffer, 0, buffer.Length)) != 0)
            {
                destination.Write(buffer, 0, read);
                total += read;
                progressChangedWatcher.OnNext(total);
            }
            progressChangedWatcher.OnCompleted();
        }

        public static async Task CopyToAsync(this Stream stream, Stream destination, IObserver<long> progressChangedWatcher)
        {
            await CopyToAsync(stream, destination, DefaultCopyBufferSize, progressChangedWatcher);
        }

        public static async Task CopyToAsync(this Stream stream, Stream destination, int bufferSize, IObserver<long> progressChangedWatcher)
        {
            if (progressChangedWatcher == null) throw new ArgumentNullException(nameof(progressChangedWatcher));

            long total = 0;
            byte[] buffer = new byte[bufferSize];
            int bytesRead;
            while ((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length).ConfigureAwait(false)) != 0)
            {
                await destination.WriteAsync(buffer, 0, bytesRead).ConfigureAwait(false);
                total += bytesRead;
                progressChangedWatcher.OnNext(total);
            }
            progressChangedWatcher.OnCompleted();
        }

        #endregion
    }
}
