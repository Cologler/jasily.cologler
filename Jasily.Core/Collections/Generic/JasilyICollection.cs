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

        public static bool MoveTo<T>(this ICollection<T> source, T item, ICollection<T> dest)
        {
            if (source.Remove(item))
            {
                dest.Add(item);
                return true;
            }

            return false;
        }
    }
}