using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Collections.Generic
{
    public static class JasilyDictionary
    {
        public static TValue GetValueOrDefault<TKey, TValue>(this Dictionary<TKey, TValue> obj, TKey key, TValue defaultValue)
        {
            TValue r = default(TValue);

            if (obj.TryGetValue(key, out r))
                return r;
            else
                return defaultValue;
        }
        public static TResult GetValueOrDefault<TKey, TValue, TResult>(this Dictionary<TKey, TValue> obj, TKey key, Func<TValue, TResult> selector, TResult defaultValue)
        {
            TValue r = default(TValue);

            if (obj.TryGetValue(key, out r))
                return selector(r);
            else
                return defaultValue;
        }
    }
}
