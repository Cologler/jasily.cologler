namespace System.Collections
{
    public struct Interval<T>
    {
        public T Left { get; }

        public T Right { get; }

        public Interval(T left, T right)
        {
            this.Left = left;
            this.Right = right;
        }
    }
}