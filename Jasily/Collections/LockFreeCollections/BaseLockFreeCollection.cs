using Jasily.Threading;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

#pragma warning disable 420

namespace Jasily.Collections.LockFreeCollections
{
    public abstract class BaseLockFreeCollection<T> : IEnumerable<T>, IDisposable
    {
        private volatile int prevAdd;
        private volatile int count;
        private volatile int isDisposed = 1;

        protected bool Add(T item)
        {
            if (this.IsDisposed) return false;

            Interlocked.Increment(ref this.prevAdd);

            if (this.IsDisposed)
            {
                Interlocked.Decrement(ref this.prevAdd);
                return false;
            }

            this.AddCore(item);

            Interlocked.Decrement(ref this.prevAdd);
            Interlocked.Increment(ref this.count);
            return true;
        }

        protected abstract void AddCore(T item);

        public bool IsDisposed => this.isDisposed < 1;

        public bool MayHasNext => this.isDisposed + this.prevAdd + this.count > 0;

        protected bool TryGet(out T value)
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

            value = this.GetCore();
            return true;
        }

        protected abstract T GetCore();

        protected bool TryGet(out T value, int millisecondsTimeout)
        {
            if (millisecondsTimeout == Timeout.Infinite)
            {
                while (!this.TryGet(out value)) { }
                return true;
            }
            else if (millisecondsTimeout < 0)
            {
                throw new ArgumentOutOfRangeException();
            }
            else if (millisecondsTimeout == 0)
            {
                return this.TryGet(out value);
            }
            else
            {

                JasilyTimeout timeout = millisecondsTimeout;
                while (true)
                {
                    var lt = timeout.LeftTime;
                    if (lt < 0) break;
                    if (lt == 0) return this.TryGet(out value);
                    if (SpinWait.SpinUntil(() => this.Count > 0, lt) && this.TryGet(out value))
                    {
                        return true;
                    }
                }
                value = default(T);
                return false;
            }
        }

        public bool TryGet(out T value, TimeSpan timeout)
            => this.TryGet(out value, (int)timeout.TotalMilliseconds);

        public int Count => this.count;

        protected class Node
        {
            private Node next;

            public T Value { get; }

            public bool HasValue { get; }

            public Node()
            {
                this.HasValue = false;
            }

            public Node(T value)
            {
                this.Value = value;
                this.HasValue = true;
            }

            public Node Next => this.next;

            public bool TrySetNext(T value)
            {
                if (this.next != null) return false;
                return Interlocked.CompareExchange(ref this.next, new Node(value), null) == null;
            }

            public bool TrySetNext(Node value)
            {
                if (this.next != null) return false;
                return Interlocked.CompareExchange(ref this.next, value, null) == null;
            }

            public bool TrySetNextEmpty()
            {
                if (this.next != null) return false;
                return Interlocked.CompareExchange(ref this.next, new Node(), null) == null;
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
            private readonly BaseLockFreeCollection<T> collection;
            private T value;

            public Enumerator(BaseLockFreeCollection<T> collection)
            {
                this.collection = collection;
            }

            public bool MoveNext()
            {
                while (this.collection.MayHasNext)
                {
                    if (this.collection.TryGet(out this.value)) return true;
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