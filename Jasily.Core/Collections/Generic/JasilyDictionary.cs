
using System.Linq;

namespace System.Collections.Generic
{
    public static class JasilyDictionary
    {
        public static TValue GetValueOrDefault<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> obj, TKey key,
            TValue defaultValue = default(TValue))
        {
            TValue r;
            return obj.TryGetValue(key, out r) ? r : defaultValue;
        }

        public static TValue GetValueOrDefault<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> obj, TKey key,
            Func<TValue> defaultValueFunc)
        {
            TValue r;
            return obj.TryGetValue(key, out r) ? r : defaultValueFunc();
        }

        public static TResult GetValueOrDefault<TKey, TValue, TResult>(this IReadOnlyDictionary<TKey, TValue> obj, TKey key,
            Func<TValue, TResult> selector, TResult defaultValue = default(TResult))
        {
            TValue r;
            return obj.TryGetValue(key, out r) ? selector(r) : defaultValue;
        }

        public static TValue GetValueOrSetDefault<TKey, TValue>(this IDictionary<TKey, TValue> obj, TKey key,
            Func<TValue> defaultValueFunc)
        {
            TValue r;

            if (!obj.TryGetValue(key, out r))
            {
                obj.Add(key, r = defaultValueFunc());
            }

            return r;
        }

        public static TValue GetValueOrSetDefault<TKey, TValue>(this IDictionary<TKey, TValue> obj, TKey key,
            TValue defaultValue)
        {
            TValue r;

            if (!obj.TryGetValue(key, out r))
            {
                obj.Add(key, r = defaultValue);
            }

            return r;
        }

        public static int RemoveRange<TKey, TValue>(this IDictionary<TKey, TValue> obj, IEnumerable<TKey> keys)
        {
            return keys.Count(obj.Remove);
        }
    }
}
