using System.Diagnostics;
using System.Threading.Tasks;

namespace System.Threading
{
    public static class SemaphoreSlimExtensions
    {
        private static Releaser<SemaphoreSlim> CreateReleaser(SemaphoreSlim semaphore)
        {
            var releaser = new Releaser<SemaphoreSlim>(true, semaphore);
            releaser.ReleaseRaised += Releaser_ReleaseRaised;
            return releaser;
        }

        private static void Releaser_ReleaseRaised(Releaser<SemaphoreSlim> sender, SemaphoreSlim e)
        {
            sender.ReleaseRaised -= Releaser_ReleaseRaised;
            Debug.Assert(e != null);
            e.Release();
        }

        public static async Task<Releaser<SemaphoreSlim>> LockAsync(this SemaphoreSlim semaphore)
        {
            await semaphore.WaitAsync();
            return CreateReleaser(semaphore);
        }

        public static async Task<Releaser<SemaphoreSlim>> LockAsync(this SemaphoreSlim semaphore,
            int millisecondsTimeout)
        {
            return await semaphore.WaitAsync(millisecondsTimeout)
                ? CreateReleaser(semaphore)
                : new Releaser<SemaphoreSlim>();
        }

        public static async Task<Releaser<SemaphoreSlim>> LockAsync(this SemaphoreSlim semaphore,
            int millisecondsTimeout, CancellationToken cancellationToken)
        {
            return await semaphore.WaitAsync(millisecondsTimeout, cancellationToken)
                ? CreateReleaser(semaphore)
                : new Releaser<SemaphoreSlim>();
        }

        public static async Task<Releaser<SemaphoreSlim>> LockAsync(this SemaphoreSlim semaphore,
            TimeSpan timeout)
        {
            return await semaphore.WaitAsync(timeout)
                ? CreateReleaser(semaphore)
                : new Releaser<SemaphoreSlim>();
        }

        public static async Task<Releaser<SemaphoreSlim>> LockAsync(this SemaphoreSlim semaphore,
            TimeSpan timeout, CancellationToken cancellationToken)
        {
            return await semaphore.WaitAsync(timeout, cancellationToken)
                ? CreateReleaser(semaphore)
                : new Releaser<SemaphoreSlim>();
        }
    }
}