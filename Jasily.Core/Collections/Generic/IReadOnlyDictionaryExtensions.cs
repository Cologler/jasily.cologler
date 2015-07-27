namespace System.Collections.Generic
{
    public static class IReadOnlyDictionaryExtensions
    {
        public static TValue GetValueOrDefault<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> obj, TKey key,
            TValue defaultValue = default(TValue))
        {
            return TryGetValueExtensions.GetValueOrDefault(obj.TryGetValue, key, defaultValue);
        }

        public static TValue GetValueOrDefault<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> obj, TKey key,
            Func<TValue> defaultValueFunc)
        {
            return TryGetValueExtensions.GetValueOrDefault(obj.TryGetValue, key, defaultValueFunc);
        }

        public static TResult GetValueOrDefault<TKey, TValue, TResult>(this IReadOnlyDictionary<TKey, TValue> obj, TKey key,
            Func<TValue, TResult> selector, TResult defaultValue = default(TResult))
        {
            return TryGetValueExtensions.GetValueOrDefault(obj.TryGetValue, key, selector, defaultValue);
        }
    }
}