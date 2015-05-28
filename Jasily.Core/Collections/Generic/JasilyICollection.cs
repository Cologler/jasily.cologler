namespace System.Collections.Generic
{
    public static class JasilyICollection
    {
        public static T Append<T>(this ICollection<T> collection, T item)
        {
            collection.Add(item);
            return item;
        }
    }
}