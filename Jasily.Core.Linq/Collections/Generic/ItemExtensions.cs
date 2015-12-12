namespace System.Collections.Generic
{
    public static class ItemExtensions
    {
        public static T BeAdd<T>(this T obj, ICollection<T> collection)
        {
            collection.Add(obj);
            return obj;
        }
    }
}