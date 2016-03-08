using System;
using System.Collections;
using System.Collections.Generic;

namespace Jasily.Collections.ReaderWriterCollections
{
    public class ReaderWriterQueue<T> : ReaderWriterCollection<T>
    {
        private readonly Queue<T> queue = new Queue<T>();

        protected override IEnumerable<T> GetBaseEnumerable() => this.queue;

        protected override ICollection GetBaseCollection() => this.queue;

        public override void Add(T item) => this.Enqueue(item);

        public override void Clear()
        {
            using (this.StartWrite())
            {
                this.queue.Clear();
            }
        }

        public override bool Contains(T item)
        {
            using (this.StartRead())
            {
                return this.queue.Contains(item);
            }
        }

        public override bool Remove(T item)
        {
            throw new NotSupportedException();
        }

        public T Dequeue()
        {
            using (this.StartWrite())
            {
                return this.queue.Dequeue();
            }
        }

        public void Enqueue(T item)
        {
            using (this.StartWrite())
            {
                this.queue.Enqueue(item);
            }
        }

        public T Peek()
        {
            using (this.StartRead())
            {
                return this.queue.Peek();
            }
        }
    }
}