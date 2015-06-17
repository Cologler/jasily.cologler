
namespace System.Collections.Generic
{
    public static class JasilyITryGetValue
    {
        public static TValue? GetValueOrNull<TKey, TValue>(this ITryGetValue<TKey, TValue> obj, TKey key)
            where TValue : struct
        {
            TValue r;

            if (obj.TryGetValue(key, out r))
                return r;
            else
                return null;
        }


        public static TValue GetValueOrDefault<TKey, TValue>(this ITryGetValue<TKey, TValue> obj, TKey key,
            TValue defaultValue = default(TValue))
        {
            var r = default(TValue);

            if (obj.TryGetValue(key, out r))
                return r;
            else
                return defaultValue;
        }


        public static TResult GetValueOrDefault<TKey, TValue, TResult>(this ITryGetValue<TKey, TValue> obj, TKey key,
            Func<TValue, TResult> selector, TResult defaultValue = default(TResult))
        {
            var r = default(TValue);

            if (obj.TryGetValue(key, out r))
                return selector(r);
            else
                return defaultValue;
        }
    }
}
