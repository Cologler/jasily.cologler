
namespace System.Cache.CacheProviders
{
    public interface IStoreProvider<TKey, TValue>
    {
        TValue this[TKey key] { get; set; }
    }
}
