using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace System.IO
{
    public static class StreamExtensions
    {
        #region to array

        /// <summary>
        /// 将流内容写入字节数组。
        /// </summary>
        /// <param name="stream"></param>
        /// <returns>新字节数组。</returns>
        public static byte[] ToArray([NotNull] this Stream stream)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            using (var ms = stream.CanSeek ? new MemoryStream(Convert.ToInt32(stream.Length)) : new MemoryStream())
            {
                if (stream.CanSeek && stream.Position != 0) stream.Position = 0;
                stream.CopyTo(ms);
                return ms.ToArray();
            }
        }

        public static byte[] ToArray(this Stream stream, CancellationToken cancellationToken)
        {
            using (var ms = stream.CanSeek ? new MemoryStream(Convert.ToInt32(stream.Length)) : new MemoryStream())
            {
                if (stream.CanSeek && stream.Position != 0) stream.Position = 0;
                stream.CopyTo(ms, cancellationToken);
                return ms.ToArray();
            }
        }

        public static async Task<byte[]> ToArrayAsync([NotNull] this Stream stream)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            using (var ms = stream.CanSeek ? new MemoryStream(Convert.ToInt32(stream.Length)) : new MemoryStream())
            {
                if (stream.CanSeek && stream.Position != 0) stream.Position = 0;
                await stream.CopyToAsync(ms);
                return ms.ToArray();
            }
        }

        public static async Task<byte[]> ToArrayAsync(this Stream stream, CancellationToken cancellationToken)
        {
            using (var ms = stream.CanSeek ? new MemoryStream(Convert.ToInt32(stream.Length)) : new MemoryStream())
            {
                if (stream.CanSeek && stream.Position != 0) stream.Position = 0;
                await stream.CopyToAsync(ms, cancellationToken);
                return ms.ToArray();
            }
        }

        #endregion

        #region read & write

        /// <summary>
        /// write whole buffer into stream
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="buffer"></param>
        public static void Write([NotNull] this Stream stream, byte[] buffer)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            stream.Write(buffer, 0, buffer.Length);
        }

        public static void Write([NotNull] this Stream stream, ArraySegment<byte> buffer)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            buffer.ThrowIfInvalid(nameof(buffer));
            stream.Write(buffer.Array, buffer.Offset, buffer.Count);
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

        public static async Task<int> ReadAsync(this Stream stream, JasilyBuffer buffer)
            => await stream.ReadAsync(buffer.Buffer, buffer.Offset, buffer.Count);

        public static async Task WriteAsync(this Stream stream, JasilyBuffer buffer)
            => await stream.WriteAsync(buffer.Buffer, buffer.Offset, buffer.Count);

        public static IEnumerable<byte[]> ReadAsEnumerable([NotNull] this Stream stream, int maxCount, bool reuseBuffer = false)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            if (maxCount < 1) throw new ArgumentOutOfRangeException(nameof(maxCount));

            var buffer = new byte[maxCount];
            int readed;
            if (reuseBuffer)
            {
                while ((readed = stream.Read(buffer, 0, maxCount)) != 0)
                {
                    yield return readed != maxCount ? buffer.ToArray(readed) : buffer;
                }
            }
            else
            {
                while ((readed = stream.Read(buffer, 0, maxCount)) != 0)
                {
                    yield return readed != maxCount ? buffer.ToArray(readed) : buffer.ToArray();
                }
            }
        }

        #endregion

        #region copy

        /// <summary>
        /// see http://referencesource.microsoft.com/#mscorlib/system/io/stream.cs,2a0f078c2e0c0aa8
        /// </summary>
        private const int DefaultCopyBufferSize = 81920;

        public static void CopyTo(this Stream stream, Stream destination, CancellationToken cancellationToken,
            int bufferSize = DefaultCopyBufferSize)
        {
            var buffer = new byte[bufferSize];
            int read;
            cancellationToken.ThrowIfCancellationRequested();
            while ((read = stream.Read(buffer, 0, buffer.Length)) != 0)
            {
                cancellationToken.ThrowIfCancellationRequested();
                destination.Write(buffer, 0, read);
                cancellationToken.ThrowIfCancellationRequested();
            }
            cancellationToken.ThrowIfCancellationRequested();
        }

        public static void CopyTo(this Stream stream, Stream destination, IObserver<long> progressChangedWatcher)
            => CopyTo(stream, destination, DefaultCopyBufferSize, progressChangedWatcher);

        public static void CopyTo(this Stream stream, Stream destination, int bufferSize, IObserver<long> progressChangedWatcher)
        {
            if (progressChangedWatcher == null) throw new ArgumentNullException(nameof(progressChangedWatcher));
            if (bufferSize <= 0) throw new ArgumentOutOfRangeException($"{nameof(bufferSize)} must >= 0.");

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

        public static async Task CopyToAsync(this Stream stream, Stream destination, CancellationToken cancellationToken)
            => await stream.CopyToAsync(destination, DefaultCopyBufferSize, cancellationToken);

        public static async Task CopyToAsync(this Stream stream, Stream destination, IObserver<long> progressChangedWatcher)
            => await CopyToAsync(stream, destination, DefaultCopyBufferSize, progressChangedWatcher);

        public static async Task CopyToAsync(this Stream stream, Stream destination, int bufferSize, IObserver<long> progressChangedWatcher)
        {
            if (progressChangedWatcher == null) throw new ArgumentNullException(nameof(progressChangedWatcher));
            if (bufferSize <= 0) throw new ArgumentOutOfRangeException($"{nameof(bufferSize)} must >= 0.");

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
        /// use two thread to read/write stream.
        /// this method was testing, no not use to release.
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="destination"></param>
        /// <param name="bufferSize"></param>
        /// <param name="progressChangedWatcher"></param>
        /// <param name="poolSize"></param>
        /// <returns></returns>
        public static async Task DoubleThreadCopyToAsync(this Stream stream, Stream destination, int bufferSize,
            IObserver<long> progressChangedWatcher, int poolSize = 20)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            if (destination == null) throw new ArgumentNullException(nameof(destination));
            if (bufferSize <= 0) throw new ArgumentOutOfRangeException($"{nameof(bufferSize)} must >= 0.");
            if (progressChangedWatcher == null) throw new ArgumentNullException(nameof(progressChangedWatcher));
            if (poolSize <= 0) throw new ArgumentOutOfRangeException($"{nameof(poolSize)} must > 0.");

            using (var copyer = new DoubleThreadStreamCopyer(stream, destination, bufferSize, poolSize, progressChangedWatcher))
                await copyer.Start();
        }

        public static async Task CopyToObserveOnTimeAsync(this Stream stream, Stream destination, int bufferSize,
            IObserver<long> progressChangedWatcher, int milliseconds)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            if (destination == null) throw new ArgumentNullException(nameof(destination));
            if (bufferSize <= 0) throw new ArgumentOutOfRangeException($"{nameof(bufferSize)} must >= 0.");
            if (progressChangedWatcher == null) throw new ArgumentNullException(nameof(progressChangedWatcher));
            if (milliseconds <= 0) throw new ArgumentOutOfRangeException($"{nameof(milliseconds)} must > 0.");

            var copyer = new StreamCopyTimeObserver(stream, destination, bufferSize, progressChangedWatcher, milliseconds);
            await copyer.StartAsync();
        }
        public static async Task CopyToObserveOnTimeAsync(this Stream stream, Stream destination, int bufferSize,
            IObserver<long> progressChangedWatcher, TimeSpan timeSpan)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            if (destination == null) throw new ArgumentNullException(nameof(destination));
            if (bufferSize <= 0) throw new ArgumentOutOfRangeException($"{nameof(bufferSize)} must >= 0.");
            if (progressChangedWatcher == null) throw new ArgumentNullException(nameof(progressChangedWatcher));
            if (timeSpan.Milliseconds <= 0) throw new ArgumentOutOfRangeException($"{nameof(timeSpan)} must > 0.");

            var copyer = new StreamCopyTimeObserver(stream, destination, bufferSize, progressChangedWatcher, timeSpan);
            await copyer.StartAsync();
        }

        public static async Task CopyToObserveOnProgressAsync(this Stream stream, Stream destination, int bufferSize,
            IObserver<long> progressChangedWatcher, int step)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            if (destination == null) throw new ArgumentNullException(nameof(destination));
            if (bufferSize <= 0) throw new ArgumentOutOfRangeException($"{nameof(bufferSize)} must >= 0.");
            if (progressChangedWatcher == null) throw new ArgumentNullException(nameof(progressChangedWatcher));
            if (step <= 0) throw new ArgumentOutOfRangeException($"{nameof(step)} must > 0.");

            var copyer = new StreamCopyProgressObserver(stream, destination, bufferSize, progressChangedWatcher, step);
            await copyer.StartAsync();
        }

        #endregion

        private class DoubleThreadStreamCopyer : IDisposable
        {
            private readonly object SyncRoot = new object();
            private readonly Queue<JasilyBuffer> BufferPool = new Queue<JasilyBuffer>();
            private readonly Stream Input;
            private readonly Stream Output;
            private readonly int BufferSize;
            private readonly int PoolSize;
            private readonly SemaphoreSlim Semaphore;
            private readonly IObserver<long> Watcher;
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

            public DoubleThreadStreamCopyer(Stream input, Stream output, int bufferSize, int poolSize = 20, IObserver<long> progressChangedWatcher = null)
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
                await Task.Run(async () =>
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

        private class StreamCopyTimeObserver : StreamCopyObserver
        {
            private TimeSpan TimeSpan;

            public StreamCopyTimeObserver(Stream input, Stream output, int bufferSize, IObserver<long> watcher,
                int milliseconds)
                : base(input, output, bufferSize, watcher)
            {
                if (milliseconds <= 0) throw new ArgumentOutOfRangeException($"{nameof(milliseconds)} must > 0.");

                this.TimeSpan = TimeSpan.FromMilliseconds(milliseconds);
            }
            public StreamCopyTimeObserver(Stream input, Stream output, int bufferSize, IObserver<long> watcher,
                TimeSpan timeSpan)
                : base(input, output, bufferSize, watcher)
            {
                if (timeSpan.Milliseconds <= 0) throw new ArgumentOutOfRangeException($"{nameof(timeSpan)} must > 0.");

                this.TimeSpan = timeSpan;
            }

            public async override Task StartAsync()
            {
                this.Timer();
                await base.StartAsync();
            }

            private void Timer()
            {
                Task.Run(async () =>
                {
                    while (!this.IsCompleted)
                    {
                        base.OnRead();
                        await Task.Delay(this.TimeSpan);
                    }
                });
            }

            protected override void OnRead()
            {

            }
        }

        private class StreamCopyProgressObserver : StreamCopyObserver
        {
            private int Step;
            private long NextFire;

            public StreamCopyProgressObserver(Stream input, Stream output, int bufferSize, IObserver<long> watcher,
                int step)
                : base(input, output, bufferSize, watcher)
            {
                if (step <= 0) throw new ArgumentOutOfRangeException($"{nameof(step)} must >= 0.");

                this.NextFire = this.Step = step;
            }

            protected override void OnRead()
            {
                if (this.TotalReaded >= this.NextFire)
                {
                    base.OnRead();
                    this.NextFire = ((this.TotalReaded / this.Step) * this.Step) + this.Step;
                }
            }
        }

        private class StreamCopyObserver
        {
            protected Stream Input { get; }
            protected Stream Output { get; }
            protected int BufferSize { get; }
            protected IObserver<long> Watcher { get; }

            protected long TotalReaded { get; private set; }
            protected bool IsCompleted { get; private set; }

            public StreamCopyObserver(Stream input, Stream output, int bufferSize, IObserver<long> watcher)
            {
                if (input == null) throw new ArgumentNullException(nameof(input));
                if (output == null) throw new ArgumentNullException(nameof(output));
                if (watcher == null) throw new ArgumentNullException(nameof(watcher));
                if (bufferSize <= 0) throw new ArgumentOutOfRangeException($"{nameof(bufferSize)} must >= 0.");

                this.Input = input;
                this.Output = output;
                this.BufferSize = bufferSize;
                this.Watcher = watcher;
            }

            public async virtual Task StartAsync()
            {
                byte[] buffer = new byte[BufferSize];
                int bytesRead;
                while ((bytesRead = await this.Input.ReadAsync(buffer, 0, buffer.Length).ConfigureAwait(false)) != 0)
                {
                    await this.Output.WriteAsync(buffer, 0, bytesRead).ConfigureAwait(false);
                    this.TotalReaded += bytesRead;
                    this.OnRead();
                }
                this.OnCompleted();
            }

            protected virtual void OnRead()
            {
                this.Watcher.OnNext(this.TotalReaded);
            }

            protected virtual void OnCompleted()
            {
                this.IsCompleted = true;
                this.Watcher.OnCompleted();
            }
        }
    }
}
