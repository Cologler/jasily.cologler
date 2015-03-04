using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Cache.CacheProviders
{
    public interface IStoreProvider<TKey, TValue>
    {
        TValue this[TKey key] { get; set; }
    }
}
