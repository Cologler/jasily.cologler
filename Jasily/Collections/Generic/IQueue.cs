namespace Jasily.Collections.Generic
{
    public interface IQueue<T>
    {
        void Enqueue(T item);

        T Dequeue();

        T Peek();

        void Clear();
    }
}