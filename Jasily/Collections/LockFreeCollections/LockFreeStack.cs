using System;
using System.Threading;

#pragma warning disable 420

namespace Jasily.Collections.LockFreeCollections
{
    /// <summary>
    /// thread-safe with lock-free queue
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class LockFreeStack<T> : BaseLockFreeCollection<T>
    {
        private volatile Node root;

        public LockFreeStack()
        {
            this.root = new Node();
        }

        public bool Push(T item) => base.Add(item);

        protected override void AddCore(T item)
        {
            while (true)
            {
                var node = new Node(item);
                node.TrySetNext(this.root);
                if (Interlocked.CompareExchange(ref this.root, node, node.Next) == node.Next)
                {
                    break;
                }
            }
        }

        protected override T GetCore()
        {
            while (true)
            {
                var root = this.root;
                if (Interlocked.CompareExchange(ref this.root, this.root.Next, root) == root)
                {
                    return root.Value;
                }
            }
        }

        public bool TryPop(out T value)
            => this.TryGet(out value);

        public bool TryPop(out T value, int millisecondsTimeout)
            => this.TryGet(out value, millisecondsTimeout);

        public bool TryPop(out T value, TimeSpan timeout)
            => this.TryGet(out value, timeout);
    }
}