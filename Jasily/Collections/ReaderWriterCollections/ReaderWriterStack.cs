using System;
using System.Collections;
using System.Collections.Generic;

namespace Jasily.Collections.ReaderWriterCollections
{
    public class ReaderWriterStack<T> : ReaderWriterCollection<T>
    {
        private readonly Stack<T> stack = new Stack<T>();

        protected override IEnumerable<T> GetBaseEnumerable() => this.stack;

        protected override ICollection GetBaseCollection() => this.stack;

        public override void Add(T item) => this.Push(item);

        public override void Clear()
        {
            using (this.StartWrite())
            {
                this.stack.Clear();
            }
        }

        public override bool Contains(T item)
        {
            using (this.StartRead())
            {
                return this.stack.Contains(item);
            }
        }

        public override bool Remove(T item)
        {
            throw new NotSupportedException();
        }

        public T Peek()
        {
            using (this.StartWrite())
            {
                return this.stack.Peek();
            }
        }

        public T Pop()
        {
            using (this.StartWrite())
            {
                return this.stack.Pop();
            }
        }

        public void Push(T item)
        {
            using (this.StartWrite())
            {
                this.stack.Push(item);
            }
        }
    }
}