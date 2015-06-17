namespace System.Collections.Generic
{
    public static class JasilyICollection
    {
        public static ICollection<T> Append<T>(this ICollection<T> collection, T item)
        {
            collection.Add(item);
            return collection;
        }

        public static T AddAndReturn<T>(this ICollection<T> collection, T item)
        {
            collection.Add(item);
            return item;
        }
    }
}