
using System.Linq;
using System.Threading.Tasks;

namespace System.Collections.Generic
{
    public static class JasilyDictionaryHelper
    {
        public static TValue GetValueOrDefault<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> obj, TKey key,
            TValue defaultValue = default(TValue))
        {
            return JasilyITryGetValue.GetValueOrDefault(obj.TryGetValue, key, defaultValue);
        }

        public static TValue GetValueOrDefault<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> obj, TKey key,
            Func<TValue> defaultValueFunc)
        {
            return JasilyITryGetValue.GetValueOrDefault(obj.TryGetValue, key, defaultValueFunc);
        }

        public static TResult GetValueOrDefault<TKey, TValue, TResult>(this IReadOnlyDictionary<TKey, TValue> obj, TKey key,
            Func<TValue, TResult> selector, TResult defaultValue = default(TResult))
        {
            return JasilyITryGetValue.GetValueOrDefault(obj.TryGetValue, key, selector, defaultValue);
        }

        public static TValue GetOrSetValue<TKey, TValue>(this IDictionary<TKey, TValue> obj, TKey key,
            Func<TValue> valueFunc)
        {
            TValue r;

            if (!obj.TryGetValue(key, out r))
            {
                obj.Add(key, r = valueFunc());
            }

            return r;
        }

        public static TValue GetOrSetValue<TKey, TValue>(this IDictionary<TKey, TValue> obj, TKey key,
            TValue value)
        {
            TValue r;

            if (!obj.TryGetValue(key, out r))
            {
                obj.Add(key, r = value);
            }

            return r;
        }

        public static int RemoveRange<TKey, TValue>(this IDictionary<TKey, TValue> obj, IEnumerable<TKey> keys)
        {
            return keys.Count(obj.Remove);
        }
    }
}
