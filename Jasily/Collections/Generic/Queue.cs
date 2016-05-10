using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace Jasily.Collections.Generic
{
    /// <summary>
    /// a reimplemented queue which don't need to copy array.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Queue<T> : IQueue<T>, IReadOnlyCollection<T>
    {
        private Node headNode;
        private Node tailNode;

        private class Node
        {
            private readonly T[] array;
            private int head;
            private int tail;

            public Node NextNode { get; private set; }

            public Node(int arraySize)
            {
                Debug.Assert(arraySize > 0);
                this.array = new T[arraySize];
            }

            public bool IsCapacityFull() => this.tail == this.array.Length - 1;

            public int Count => this.tail - this.head;

            /// <summary>
            /// return tail node.
            /// </summary>
            /// <param name="item"></param>
            /// <returns></returns>
            public Node Enqueue(T item)
            {
                if (this.IsCapacityFull())
                {
                    this.NextNode = new Node(Math.Min(this.array.Length * 2 + 1, 1024));
                    this.NextNode.Enqueue(item);
                    return this.NextNode;
                }
                else
                {
                    this.tail++;
                    this.array[this.tail] = item;
                    return this;
                }
            }

            public T Dequeue()
            {
                Debug.Assert(this.Count > 0);
                var ret = this.array[this.head];
                this.array[this.head] = default(T);
                this.head++;
                return ret;
            }

            public T Peek()
            {
                Debug.Assert(this.Count > 0);
                return this.array[this.head];
            }

            public IEnumerable<T> Enumerate()
            {
                for (var i = this.head; i <= this.tail; i++)
                {
                    yield return this.array[i];
                }
            }
        }

        public int Count { get; private set; }

        #region Implementation of IEnumerable

        /// <summary>返回一个循环访问集合的枚举器。</summary>
        /// <returns>可用于循环访问集合的 <see cref="T:System.Collections.Generic.IEnumerator`1" />。</returns>
        public IEnumerator<T> GetEnumerator()
        {
            var node = this.headNode;
            while (node != null)
            {
                foreach (var item in node.Enumerate()) yield return item;
                node = node.NextNode;
            }
        }

        /// <summary>返回一个循环访问集合的枚举器。</summary>
        /// <returns>可用于循环访问集合的 <see cref="T:System.Collections.IEnumerator" /> 对象。</returns>
        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

        #endregion

        #region Implementation of IQueue<T>

        public void Enqueue(T item)
        {
            if (this.tailNode == null)
            {
                Debug.Assert(this.headNode == null);
                this.headNode = this.tailNode = new Node(3);
            }

            this.tailNode = this.tailNode.Enqueue(item);
            this.Count++;
        }

        public T Dequeue()
        {
            if (this.Count == 0) throw new InvalidOperationException();
            Debug.Assert(this.headNode != null);
            Debug.Assert(this.tailNode != null);

            var result = this.headNode.Dequeue();
            this.Count--;
            if (this.Count == 0)
            {
                Debug.Assert(this.headNode == this.tailNode);
                Debug.Assert(this.headNode.Count == 0);
                this.Clear();
            }
            else if (this.headNode.Count == 0)
            {
                Debug.Assert(this.headNode.IsCapacityFull());
                Debug.Assert(this.headNode.NextNode != null);
                this.headNode = this.headNode.NextNode;
            }

            return result;
        }

        public T Peek()
        {
            if (this.Count == 0) throw new InvalidOperationException();
            Debug.Assert(this.headNode != null);
            Debug.Assert(this.tailNode != null);
            return this.headNode.Peek();
        }

        public void Clear()
        {
            this.Count = 0;
            this.headNode = this.tailNode = null;
        }

        #endregion
    }
}