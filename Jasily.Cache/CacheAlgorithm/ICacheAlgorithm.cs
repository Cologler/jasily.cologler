
namespace System.Cache.CacheAlgorithm
{
    public interface ICacheAlgorithm<TKey>
    {
        bool IsNeedAddToStore(TKey key);

        event EventHandler<TKey> RemoveFromStore;
    }
}
