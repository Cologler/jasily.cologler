namespace System.Collections.Generic
{
    public static class TryGetValueExtensions
    {
        public static TValue? GetValueOrNull<TKey, TValue>(this TryGetValueDelegate<TKey, TValue> func, TKey key)
            where TValue : struct
        {
            if (func == null) throw new ArgumentNullException(nameof(func));

            TValue r;
            return func(key, out r) ? r : (TValue?)null;
        }

        public static TValue GetValueOrDefault<TKey, TValue>(this TryGetValueDelegate<TKey, TValue> func, TKey key,
            TValue defaultValue = default(TValue))
        {
            if (func == null) throw new ArgumentNullException(nameof(func));

            TValue r;

            return func(key, out r) ? r : defaultValue;
        }

        public static TValue GetValueOrDefault<TKey, TValue>(this TryGetValueDelegate<TKey, TValue> func, TKey key,
            Func<TValue> defaultValueFunc)
        {
            if (func == null) throw new ArgumentNullException(nameof(func));
            if (defaultValueFunc == null) throw new ArgumentNullException(nameof(defaultValueFunc));

            TValue r;
            return func(key, out r) ? r : defaultValueFunc();
        }

        public static TResult GetValueOrDefault<TKey, TValue, TResult>(this TryGetValueDelegate<TKey, TValue> func, TKey key,
            Func<TValue, TResult> selector, TResult defaultResult = default(TResult))
        {
            if (func == null) throw new ArgumentNullException(nameof(func));

            TValue r;
            return func(key, out r) ? selector(r) : defaultResult;
        }
    }
}