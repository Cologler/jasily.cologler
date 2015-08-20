
namespace System.Collections.Generic
{
    public static class ITryGetValueExtensions
    {
        public static TValue? GetValueOrNull<TKey, TValue>(this ITryGetValue<TKey, TValue> obj, TKey key)
            where TValue : struct
        {
            return TryGetValueExtensions.GetValueOrNull<TKey, TValue>(obj.TryGetValue, key);
        }

        public static TValue GetValueOrDefault<TKey, TValue>(this ITryGetValue<TKey, TValue> obj, TKey key,
            TValue defaultValue = default(TValue))
        {
            return TryGetValueExtensions.GetValueOrDefault(obj.TryGetValue, key, defaultValue);
        }

        public static TResult GetValueOrDefault<TKey, TValue, TResult>(this ITryGetValue<TKey, TValue> obj, TKey key,
            Func<TValue, TResult> selector, TResult defaultValue = default(TResult))
        {
            return TryGetValueExtensions.GetValueOrDefault(obj.TryGetValue, key, selector, defaultValue);
        }
    }
}
