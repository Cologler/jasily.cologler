using System;
using System.Threading;

namespace Jasily.Threading.LockFree
{
    /// <summary>
    /// a simple unwaitable & lockfree semaphore
    /// </summary>
    public class LockFreeSemaphore : ISemaphore
    {
        private readonly int maxCount;
        private int currentCount;

        public int CurrentCount => this.currentCount;

        public LockFreeSemaphore()
            : this(1, 1)
        {
        }

        public LockFreeSemaphore(int initialCount)
            : this(initialCount, initialCount)
        {

        }

        public LockFreeSemaphore(int initialCount, int maxCount)
        {
            if (initialCount < 0 || maxCount == 0 || initialCount > maxCount) throw new ArgumentOutOfRangeException();
            this.currentCount = initialCount;
            this.maxCount = maxCount;
        }

        public int Release(int count = 1)
        {
            if (count < 1) throw new ArgumentOutOfRangeException();
            while (true)
            {
                var current = this.currentCount;
                if (current + count > this.maxCount) throw new SemaphoreFullException();
                if (Interlocked.CompareExchange(ref this.currentCount, current + count, current) == current)
                    return current;
            }
        }

        public Releaser<int> Acquire(int count = 1)
        {
            if (count < 1) throw new ArgumentOutOfRangeException();
            while (true)
            {
                var current = this.currentCount;
                if (count > current) return new Releaser<int>();
                if (Interlocked.CompareExchange(ref this.currentCount, current - count, current) == current)
                {
                    var locker = new Releaser<int>(true, count);
                    locker.ReleaseRaised += this.Locker_ReleaseRaised;
                    return locker;
                }
            }
        }

        private void Locker_ReleaseRaised(Releaser<int> sender, int e)
        {
            sender.ReleaseRaised -= this.Locker_ReleaseRaised;
            this.Release(e);
        }
    }
}