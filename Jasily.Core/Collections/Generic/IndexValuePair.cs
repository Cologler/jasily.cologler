namespace System.Collections.Generic
{
    public static class IndexValuePair
    {
        public static IndexValuePair<T> From<T>(IList<T> list, int index) => new IndexValuePair<T>(index, list[index]);
    }

    public struct IndexValuePair<T>
    {
        public IndexValuePair(int index, T value)
        {
            this.Index = index;
            this.Value = value;
        }

        public int Index { get; }

        public T Value { get; }
    }
}