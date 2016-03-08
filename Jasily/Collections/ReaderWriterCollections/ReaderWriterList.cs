using System;
using System.Collections;
using System.Collections.Generic;

namespace Jasily.Collections.ReaderWriterCollections
{
    public class ReaderWriterList<T> : ReaderWriterCollection<T>,
        IList<T>, IReadOnlyList<T>, IList
    {
        private readonly List<T> list = new List<T>();

        protected override IEnumerable<T> GetBaseEnumerable() => this.list;

        protected override ICollection GetBaseCollection() => this.list;

        public override void Add(T item)
        {
            using (this.StartWrite())
            {
                this.list.Add(item);
            }
        }

        int IList.Add(object value)
        {
            if (value is T)
            {
                using (this.StartWrite())
                {
                    this.Add((T)value);
                    return this.list.Count - 1;
                }
            }
            else
            {
                return -1;
            }
        }

        public override void Clear()
        {
            using (this.StartWrite())
            {
                this.list.Clear();
            }
        }

        bool IList.Contains(object value)
        {
            if (value is T)
            {
                return this.Contains((T)value);
            }
            else
            {
                return false;
            }
        }

        public int IndexOf(object value)
        {
            if (value is T)
            {
                return this.IndexOf((T)value);
            }
            else
            {
                return -1;
            }
        }

        public void Insert(int index, object value)
        {
            if (value is T)
            {
                this.Insert(index, (T)value);
            }
            else
            {
                throw new ArgumentException();
            }
        }

        public void Remove(object value)
        {
            if (value is T)
            {
                this.Remove((T)value);
            }
        }

        public override bool Contains(T item)
        {
            using (this.StartRead())
            {
                return this.list.Contains(item);
            }
        }

        public override void CopyTo(T[] array, int arrayIndex)
        {
            using (this.StartRead())
            {
                this.list.CopyTo(array, arrayIndex);
            }
        }

        public override bool Remove(T item)
        {
            using (this.StartWrite())
            {
                return this.list.Remove(item);
            }
        }

        object IList.this[int index]
        {
            get { return this[index]; }
            set
            {
                if (value is T)
                {
                    this[index] = (T)value;
                }
                else
                {
                    throw new ArgumentException();
                }
            }
        }

        public int IndexOf(T item)
        {
            using (this.StartRead())
            {
                return this.list.IndexOf(item);
            }
        }

        public void Insert(int index, T item)
        {
            using (this.StartWrite())
            {
                this.list.Insert(index, item);
            }
        }

        public void RemoveAt(int index)
        {
            using (this.StartWrite())
            {
                this.list.RemoveAt(index);
            }
        }

        public bool IsFixedSize => false;

        public T this[int index]
        {
            get
            {
                using (this.StartRead())
                {
                    return this.list[index];
                }
            }
            set
            {
                using (this.StartWrite())
                {
                    this.list[index] = value;
                }
            }
        }
    }
}