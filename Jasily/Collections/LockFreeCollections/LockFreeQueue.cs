using System;
using System.Diagnostics;
using System.Threading;

#pragma warning disable 420

namespace Jasily.Collections.LockFreeCollections
{
    /// <summary>
    /// thread-safe with lock-free queue.
    /// it was slow then ConcurrentQueue on performance
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class LockFreeQueue<T> : BaseLockFreeCollection<T>
    {
        private volatile Node header;
        private volatile Node footer;

        public LockFreeQueue()
        {
            this.footer = this.header = new Node();
        }

        public bool Enqueue(T item) => base.Add(item);

        protected override void AddCore(T item)
        {
            while (true)
            {
                var footer = this.footer;
                if (footer.TrySetNext(item))
                {
                    this.footer = footer.Next;
                    break;
                }
            }
        }

        protected override T GetCore()
        {
            while (true)
            {
                var header = this.header;

                var next = header;
                while (!next.HasValue)
                {
                    next = next.Next;
                    Debug.Assert(next != null); // must contain a next has value.
                }

                if (next.TrySetNextEmpty())
                {
                    this.footer = this.footer.Next;
                }
                if (Interlocked.CompareExchange(ref this.header, next.Next, header) == header)
                {
                    return next.Value;
                }
            }
        }

        public bool TryDequeue(out T value)
            => this.TryGet(out value);

        public bool TryDequeue(out T value, int millisecondsTimeout)
            => this.TryGet(out value, millisecondsTimeout);

        public bool TryDequeue(out T value, TimeSpan timeout)
            => this.TryGet(out value, timeout);
    }
}