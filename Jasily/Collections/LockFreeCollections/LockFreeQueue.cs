using Jasily.Threading;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

#pragma warning disable 420

namespace Jasily.Collections.LockFreeCollections
{
    /// <summary>
    /// thread-safe with lock-free queue
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class LockFreeQueue<T> : IEnumerable<T>, IDisposable
    {
        private volatile QueueData header;
        private volatile int prevAdd;
        private volatile int count;
        private volatile QueueData footer;
        private volatile int isDisposed = 1;

        public LockFreeQueue()
        {
            this.footer = this.header = new QueueData();
        }

        public bool Enqueue(T item)
        {
            if (this.IsDisposed) return false;

            Interlocked.Increment(ref this.prevAdd);

            if (this.IsDisposed)
            {
                Interlocked.Decrement(ref this.prevAdd);
                return false;
            }

            while (true)
            {
                var footer = this.footer;
                if (footer.TrySetNext(item))
                {
                    this.footer = this.footer.Next;
                    break;
                }
            }

            Interlocked.Decrement(ref this.prevAdd);
            Interlocked.Increment(ref this.count);
            return true;
        }

        public bool IsDisposed => this.isDisposed < 1;

        public bool MayHasNext => this.isDisposed + this.prevAdd + this.count > 0;

        public bool TryDequeue(out T value)
        {
            int count;
            do
            {
                count = this.count;
                if (count <= 0)
                {
                    value = default(T);
                    return false;
                }
            } while (Interlocked.CompareExchange(ref this.count, count - 1, count) != count);

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
                    value = next.Value;
                    return true;
                }
            }
        }

        public bool TryDequeue(out T value, int millisecondsTimeout)
        {
            if (millisecondsTimeout == Timeout.Infinite)
            {
                while (!this.TryDequeue(out value)) { }
                return true;
            }
            else if (millisecondsTimeout < 0)
            {
                throw new ArgumentOutOfRangeException();
            }
            else if (millisecondsTimeout == 0)
            {
                return this.TryDequeue(out value);
            }
            else
            {

                JasilyTimeout timeout = millisecondsTimeout;
                while (true)
                {
                    var lt = timeout.LeftTime;
                    if (lt < 0) break;
                    if (lt == 0) return this.TryDequeue(out value);
                    if (SpinWait.SpinUntil(() => this.Count > 0, lt) && this.TryDequeue(out value))
                    {
                        return true;
                    }
                }
                value = default(T);
                return false;
            }
        }

        public bool TryDequeue(out T value, TimeSpan timeout)
            => this.TryDequeue(out value, (int)timeout.TotalMilliseconds);

        public int Count => this.count;

        private class QueueData
        {
            private QueueData next;

            public T Value { get; }

            public bool HasValue { get; }

            public QueueData()
            {
                this.HasValue = false;
            }

            public QueueData(T value)
            {
                this.Value = value;
                this.HasValue = true;
            }

            public QueueData Next => this.next;

            public bool TrySetNext(T value)
            {
                if (this.next != null) return false;
                return Interlocked.CompareExchange(ref this.next, new QueueData(value), null) == null;
            }

            public bool TrySetNextEmpty()
            {
                if (this.next != null) return false;
                return Interlocked.CompareExchange(ref this.next, new QueueData(), null) == null;
            }
        }

        public void Dispose()
        {
            this.isDisposed = 0;
        }

        public IEnumerator<T> GetEnumerator() => new Enumerator(this);

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

        private class Enumerator : IEnumerator<T>
        {
            private readonly LockFreeQueue<T> queue;
            private T value;

            public Enumerator(LockFreeQueue<T> queue)
            {
                this.queue = queue;
            }

            public bool MoveNext()
            {
                while (this.queue.MayHasNext)
                {
                    if (this.queue.TryDequeue(out this.value)) return true;
                }

                return false;
            }

            public void Reset()
            {
            }

            public T Current => this.value;

            object IEnumerator.Current => this.Current;

            public void Dispose()
            {
            }
        }
    }
}