using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Cache.CacheAlgorithm
{
    public interface ICacheAlgorithm<TKey>
    {
        bool IsNeedAddToStore(TKey key);

        event EventHandler<TKey> RemoveFromStore;
    }
}
