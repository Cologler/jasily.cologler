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

        public struct Releaser : IDisposable
        {
            private readonly SemaphoreSlim semaphore;

            internal Releaser(SemaphoreSlim semaphore) { this.semaphore = semaphore; }

            public void Dispose() => this.semaphore?.Release();
        }
    }
}