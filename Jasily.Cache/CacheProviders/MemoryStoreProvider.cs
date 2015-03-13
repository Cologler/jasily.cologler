using System.Collections.Generic;

namespace System.Cache.CacheProviders
{
    public sealed class MemoryStoreProvider<TKey, TValue> : IStoreProvider<TKey, TValue>
    {
        private Dictionary<TKey, TValue> InnerStore;

        public MemoryStoreProvider()
        {
            InnerStore = new Dictionary<TKey, TValue>();
        }

        public TValue this[TKey key]
        {
            get
            {
                TValue value;
                if (InnerStore.TryGetValue(key, out value))
                    return value;
                else
                    throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
    }
}
