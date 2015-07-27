
namespace System.Collections.Generic
{
    public static class ITryGetValueExtensions
    {
        public delegate bool TryGetVauleDelegate<in TKey, TValue>(TKey key, out TValue value);

        public static TValue? GetValueOrNull<TKey, TValue>(this ITryGetValue<TKey, TValue> obj, TKey key)
            where TValue : struct
        {
            return GetValueOrNull<TKey, TValue>(obj.TryGetValue, key);
        }

        public static TValue GetValueOrDefault<TKey, TValue>(this ITryGetValue<TKey, TValue> obj, TKey key,
            TValue defaultValue = default(TValue))
        {
            return GetValueOrDefault(obj.TryGetValue, key, defaultValue);
        }

        public static TResult GetValueOrDefault<TKey, TValue, TResult>(this ITryGetValue<TKey, TValue> obj, TKey key,
            Func<TValue, TResult> selector, TResult defaultValue = default(TResult))
        {
            return GetValueOrDefault(obj.TryGetValue, key, selector, defaultValue);
        }

        public static TValue? GetValueOrNull<TKey, TValue>(this TryGetVauleDelegate<TKey, TValue> func, TKey key)
            where TValue : struct
        {
            if (func == null) throw new ArgumentNullException("func");

            TValue r;
            return func(key, out r) ? r : (TValue?) null;
        }

        public static TValue GetValueOrDefault<TKey, TValue>(this TryGetVauleDelegate<TKey, TValue> func, TKey key,
            TValue defaultValue = default(TValue))
        {
            if (func == null) throw new ArgumentNullException("func");

            TValue r;

            return func(key, out r) ? r : defaultValue;
        }

        public static TValue GetValueOrDefault<TKey, TValue>(this TryGetVauleDelegate<TKey, TValue> func, TKey key,
            Func<TValue> defaultValueFunc)
        {
            if (func == null) throw new ArgumentNullException("func");
            if (defaultValueFunc == null) throw new ArgumentNullException("defaultValueFunc");

            TValue r;
            return func(key, out r) ? r : defaultValueFunc();
        }

        public static TResult GetValueOrDefault<TKey, TValue, TResult>(this TryGetVauleDelegate<TKey, TValue> func, TKey key,
            Func<TValue, TResult> selector, TResult defaultResult = default(TResult))
        {
            if (func == null) throw new ArgumentNullException("func");

            TValue r;
            return func(key, out r) ? selector(r) : defaultResult;
        }
    }
}
