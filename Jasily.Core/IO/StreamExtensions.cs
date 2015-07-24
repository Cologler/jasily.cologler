using System.Collections.Generic;
using System.Diagnostics;
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

        #region read & write

        public static async Task<int> ReadAsync(this Stream stream, JasilyBuffer buffer)
            => await stream.ReadAsync(buffer.Buffer, buffer.Offset, buffer.Count);

        public static async Task WriteAsync(this Stream stream, JasilyBuffer buffer)
            => await stream.WriteAsync(buffer.Buffer, buffer.Offset, buffer.Count);

        #endregion

        #region copy

        /// <summary>
        /// see http://referencesource.microsoft.com/#mscorlib/system/io/stream.cs,2a0f078c2e0c0aa8
        /// </summary>
        private const int DefaultCopyBufferSize = 81920;

        public static void CopyTo(this Stream stream, Stream destination, IObserver<long> progressChangedWatcher)
            => CopyTo(stream, destination, DefaultCopyBufferSize, progressChangedWatcher);

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
            => await CopyToAsync(stream, destination, DefaultCopyBufferSize, progressChangedWatcher);

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

        /// <summary>
        /// this method was testing, no not use to release.
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="destination"></param>
        /// <param name="bufferSize"></param>
        /// <param name="progressChangedWatcher"></param>
        /// <param name="poolSize"></param>
        /// <returns></returns>
        public static async Task ReadWriteAsyncCopyToAsync(this Stream stream,
            Stream destination, int bufferSize, IObserver<long> progressChangedWatcher, int poolSize = 20)
        {
            if (progressChangedWatcher == null) throw new ArgumentNullException(nameof(progressChangedWatcher));

            var copyer = new StreamCopyer(stream, destination, bufferSize, poolSize, progressChangedWatcher);
            await copyer.Start();
        }

        private class StreamCopyer : IDisposable
        {
            private readonly object SyncRoot = new object();
            private readonly Queue<JasilyBuffer> BufferPool = new Queue<JasilyBuffer>();
            private readonly Stream Input;
            private readonly Stream Output;
            private readonly int BufferSize;
            private readonly int PoolSize;
            private readonly SemaphoreSlim Semaphore;
            private readonly IObserver<long>  Watcher;
            private readonly TaskCompletionSource<bool> TaskInstance = new TaskCompletionSource<bool>();

            /// <summary>
            /// after IsInEndOrThrow become true, never have new buffer.
            /// </summary>
            private bool IsInEndOrThrow = false;
            private Exception InThrow;

            /// <summary>
            /// after IsOutEndOrThrow become true, never use exists buffer.
            /// </summary>
            private bool IsOutEndOrThrow = false;

            public StreamCopyer(Stream input, Stream output, int bufferSize, int poolSize = 20, IObserver<long> progressChangedWatcher = null)
            {
                this.Input = input;
                this.Output = output;
                this.BufferSize = bufferSize;
                this.PoolSize = poolSize;
                this.Watcher = progressChangedWatcher;

                // start was full.
                this.Semaphore = new SemaphoreSlim(0, poolSize);
            }

            public Task Start()
            {
                this.BeginRead();
                this.BeginWrite();
                return this.TaskInstance.Task;
            }

            private async void BeginRead()
            {
                await Task.Run(async () =>
                {
                    try
                    {
                        await this.ReadAsync();
                    }
                    catch (Exception e)
                    {
                        this.InThrow = e;
                    }
                    finally
                    {
                        this.IsInEndOrThrow = true;
                        this.End();
                    }
                });
            }

            private async Task ReadAsync()
            {
                while (true)
                {
                    // 1. read
                    var buffer = new byte[BufferSize];
                    var readed = await this.Input.ReadAsync(buffer, 0, buffer.Length).ConfigureAwait(false);
                    if (readed == 0)
                    {
                        // read to end, release all semaphore
                        this.Semaphore.Release(this.Semaphore.CurrentCount);
                        Debug.WriteLine(this.Semaphore.CurrentCount);
                        return;
                    }

                    // 2. enter semaphore
                    while (this.Semaphore.CurrentCount == this.PoolSize)
                    {
                        if (this.IsOutEndOrThrow) return;
                        await Task.Delay(100);
                    }

                    Debug.Assert(this.Semaphore.CurrentCount < this.PoolSize);

                    // 3. added buffer
                    lock (this.SyncRoot)
                    {
                        this.BufferPool.Enqueue(new JasilyBuffer(buffer, 0, readed));
                    }

                    // 4. release semaphore, then can Out()
                    this.Semaphore.Release();
                    Debug.WriteLine(this.Semaphore.CurrentCount);
                }
            }

            private async void BeginWrite()
            {
                await Task.Run(async() =>
                {
                    try
                    {
                        await this.WriteAsync();

                        if (this.InThrow != null)
                            this.TaskInstance.TrySetException(this.InThrow);
                        else
                            this.TaskInstance.TrySetResult(true);
                    }
                    catch (Exception e)
                    {
                        this.TaskInstance.TrySetException(e);
                    }
                    finally
                    {
                        this.IsOutEndOrThrow = true;
                        this.End();
                    }
                });
            }

            private async Task WriteAsync()
            {
                long total = 0;

                while (true)
                {
                    // 1. wait a semaphore
                    await this.Semaphore.WaitAsync();
                    Debug.WriteLine(this.Semaphore.CurrentCount);

                    // 2. get buffer
                    do
                    {
                        JasilyBuffer buffer;
                        lock (this.SyncRoot)
                        {
                            if (this.BufferPool.Count == 0)
                                return;

                            buffer = this.BufferPool.Dequeue();
                        }

                        // 3. flush to output
                        if (buffer.Count > 0)
                        {
                            await this.Output.WriteAsync(buffer).ConfigureAwait(false);
                            total += buffer.Count;
                            this.Watcher?.OnNext(total);
                        }
                    } while (this.IsInEndOrThrow);
                }
            }

            private void End()
            {
                if (this.IsInEndOrThrow && this.IsOutEndOrThrow)
                    this.Dispose();
            }

            public void Dispose()
            {
                this.Semaphore?.Dispose();
                this.Watcher?.OnCompleted();
            }
        }

        #endregion
    }
}
