
namespace System.Collections.Generic
{
    public static class JasilyDictionary
    {
        public static TValue GetValueOrDefault<TKey, TValue>(
            this IReadOnlyDictionary<TKey, TValue> obj, TKey key,
            TValue defaultValue = default(TValue))
        {
            TValue r;
            return obj.TryGetValue(key, out r) ? r : defaultValue;
        }

        public static TResult GetValueOrDefault<TKey, TValue, TResult>(
            this IReadOnlyDictionary<TKey, TValue> obj, TKey key,
            Func<TValue, TResult> selector,
            TResult defaultValue = default(TResult))
        {
            TValue r;
            return obj.TryGetValue(key, out r) ? selector(r) : defaultValue;
        }
    }
}
