using System;
using System.Diagnostics;
using System.Threading;

namespace Jasily.Threading
{
    /// <summary>
    /// this is unwaitable semaphore
    /// </summary>
    public abstract class JasilySemaphore
    {
        public int Count { get; }

        public abstract int Current { get; }

        protected JasilySemaphore(int count)
        {
            if (count <= 0) throw new ArgumentOutOfRangeException(nameof(count));

            this.Count = count;
        }

        public Lock TryGetLock()
        {
            return this.OnGet() ? new Lock(this) : new Lock();
        }

        protected abstract bool OnGet();

        protected abstract void OnPut();

        public struct Lock : IDisposable
        {
            private readonly JasilySemaphore semaphore;

            internal Lock(JasilySemaphore semaphore)
            {
                this.semaphore = semaphore;
            }

            public bool IsEntered => this.semaphore != null;

            public void Dispose() => this.semaphore?.OnPut();
        }

        /// <summary>
        /// general use in UI thread.
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public static JasilySemaphore CreateThreadNotSafe(int count = 1)
            => new ThreadNotSafeEnvironment(count);

        public static JasilySemaphore CreateThreadSafe(int count = 1)
            => new ThreadSafeEnvironment(count);

        private class ThreadNotSafeEnvironment : JasilySemaphore
        {
            private int current;

            public ThreadNotSafeEnvironment(int count)
                : base(count)
            {
            }

            public override int Current => this.current;

            protected override bool OnGet()
            {
                if (this.current >= this.Count) return false;
                this.current++;
                return true;
            }

            protected override void OnPut()
            {
                this.current--;
                Debug.Assert(this.current >= 0, "you need use ThreadSafe version");
            }
        }

        private class ThreadSafeEnvironment : JasilySemaphore
        {
            private int current;

            public ThreadSafeEnvironment(int count)
                : base(count)
            {
            }

            public override int Current => this.current;

            protected override bool OnGet()
            {
                if (this.current >= this.Count) return false;
                if (Interlocked.Increment(ref this.current) > this.Count)
                {
                    Interlocked.Decrement(ref this.current);
                    return false;
                }
                return true;
            }

            protected override void OnPut()
            {
                Interlocked.Decrement(ref this.current);
                Debug.Assert(this.current >= 0);
            }
        }
    }
}