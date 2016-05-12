namespace System.Collections.Generic
{
    public static class Enumerable2
    {
        public static object GetOrCreateSyncRoot<T>(this IEnumerable<T> enumerable)
        {
            return (enumerable as ICollection)?.SyncRoot ?? new object();
        }
    }
}