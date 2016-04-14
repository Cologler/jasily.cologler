using System.Threading.Tasks;

namespace System.Threading
{
    public static class SemaphoreSlimExtensions
    {
        public static async Task<Releaser> LockAsync(this SemaphoreSlim semaphore)
        {
            await semaphore.WaitAsync();
            return new Releaser(semaphore);
        }

        public static async Task<Releaser> LockAsync(this SemaphoreSlim semaphore,
            int millisecondsTimeout)
        {
            return await semaphore.WaitAsync(millisecondsTimeout)
                ? new Releaser(semaphore)
                : new Releaser();
        }

        public static async Task<Releaser> LockAsync(this SemaphoreSlim semaphore,
            int millisecondsTimeout, CancellationToken cancellationToken)
        {
            return await semaphore.WaitAsync(millisecondsTimeout, cancellationToken)
                ? new Releaser(semaphore)
                : new Releaser();
        }

        public static async Task<Releaser> LockAsync(this SemaphoreSlim semaphore,
            TimeSpan timeout)
        {
            return await semaphore.WaitAsync(timeout)
                ? new Releaser(semaphore)
                : new Releaser();
        }

        public static async Task<Releaser> LockAsync(this SemaphoreSlim semaphore,
            TimeSpan timeout, CancellationToken cancellationToken)
        {
            return await semaphore.WaitAsync(timeout, cancellationToken)
                ? new Releaser(semaphore)
                : new Releaser();
        }

        public struct Releaser : IDisposable
        {
            private readonly SemaphoreSlim semaphore;

            internal Releaser(SemaphoreSlim semaphore)
            {
                this.semaphore = semaphore;
                this.isDisposed = false;
            }

            public bool IsEntered => this.semaphore != null;

            private bool isDisposed;

            public void Dispose()
            {
                if (this.isDisposed) throw new ObjectDisposedException(nameof(Releaser));
                this.isDisposed = true;
                this.semaphore?.Release();
            }
        }
    }
}