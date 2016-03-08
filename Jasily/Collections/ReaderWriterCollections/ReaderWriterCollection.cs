using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using JetBrains.Annotations;

namespace Jasily.Collections.ReaderWriterCollections
{
    public abstract class ReaderWriterCollection<T>
        : ICollection<T>, ICollection
    {
        private readonly ReaderWriterLockSlim @lock = new ReaderWriterLockSlim();

        protected IDisposable StartRead() => this.@lock.StartRead();

        protected IDisposable StartWrite() => this.@lock.StartWrite();

        protected abstract IEnumerable<T> GetBaseEnumerable();

        protected abstract ICollection GetBaseCollection();

        public IEnumerator<T> GetEnumerator() => this.ToList().GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

        public void CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }

        public abstract void Add(T item);

        public abstract void Clear();

        public abstract bool Contains(T item);

        public virtual void CopyTo([NotNull] T[] array, int arrayIndex)
        {
            if (array == null) throw new ArgumentNullException(nameof(array));
            this.ToList().CopyTo(array, arrayIndex);
        }

        public abstract bool Remove(T item);

        public int Count => this.GetBaseCollection().Count;

        public bool IsReadOnly => false;

        public bool IsSynchronized => true;

        public object SyncRoot => this.@lock;

        public T[] ToArray()
        {
            using (this.StartRead())
            {
                return this.GetBaseEnumerable().ToArray();
            }
        }

        public List<T> ToList()
        {
            using (this.StartRead())
            {
                return this.GetBaseEnumerable().ToList();
            }
        }
    }
}