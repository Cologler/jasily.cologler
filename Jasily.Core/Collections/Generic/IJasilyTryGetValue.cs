
namespace System.Collections.Generic
{
    public interface IJasilyTryGetValue<TKey, TValue>
    {
        bool TryGetValue(TKey key, out TValue value);
    }

    public static class IJasilyTryGetValueEM
    {
        public static TValue GetValueOrDefault<TKey, TValue>(
            this IJasilyTryGetValue<TKey, TValue> obj, TKey key,
            TValue defaultValue = default(TValue))
        {
            TValue r = default(TValue);

            if (obj.TryGetValue(key, out r))
                return r;
            else
                return defaultValue;
        }
        public static TResult GetValueOrDefault<TKey, TValue, TResult>(
            this IJasilyTryGetValue<TKey, TValue> obj, TKey key,
            Func<TValue, TResult> selector,
            TResult defaultValue = default(TResult))
        {
            TValue r = default(TValue);

            if (obj.TryGetValue(key, out r))
                return selector(r);
            else
                return defaultValue;
        }
    }
}
