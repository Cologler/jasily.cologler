using System;
using System.Threading;

namespace Jasily.Threading
{
    /// <summary>
    /// use on UI thread or threadlocal.
    /// </summary>
    public class ThreadUnsafeSemaphore : ISemaphore
    {
        private readonly int maxCount;
        private int currentCount;

        public int CurrentCount => this.currentCount;

        public ThreadUnsafeSemaphore()
            : this(1, 1)
        {
        }

        public ThreadUnsafeSemaphore(int initialCount)
            : this(initialCount, initialCount)
        {

        }

        public ThreadUnsafeSemaphore(int initialCount, int maxCount)
        {
            if (initialCount < 0 || maxCount == 0 || initialCount > maxCount) throw new ArgumentOutOfRangeException();
            this.currentCount = initialCount;
            this.maxCount = maxCount;
        }

        public int Release(int count = 1)
        {
            if (count < 1) throw new ArgumentOutOfRangeException();
            if (this.currentCount + count > this.maxCount) throw new SemaphoreFullException();
            this.currentCount += count;
            return this.currentCount - count;
        }

        public Releaser<int> Acquire(int count = 1)
        {
            if (count < 1) throw new ArgumentOutOfRangeException();
            if (count > this.currentCount) return new Releaser<int>();
            this.currentCount -= count;
            var locker = new Releaser<int>(true, count);
            locker.ReleaseRaised += this.Locker_ReleaseRaised;
            return locker;
        }

        private void Locker_ReleaseRaised(Releaser<int> sender, int e)
        {
            sender.ReleaseRaised -= this.Locker_ReleaseRaised;
            this.Release(e);
        }
    }
}