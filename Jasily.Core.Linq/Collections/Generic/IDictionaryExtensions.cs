
using System.Linq;

namespace System.Collections.Generic
{
    public static class IDictionaryExtensions
    {
        #region get value or default

        public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key,
            TValue defaultValue = default(TValue))
        {
            TValue value;
            return dict.TryGetValue(key, out value) ? value : defaultValue;
        }

        public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key,
            Func<TValue> defaultValueFactory)
        {
            if (defaultValueFactory == null) throw new ArgumentNullException(nameof(defaultValueFactory));

            TValue value;
            return dict.TryGetValue(key, out value) ? value : defaultValueFactory();
        }

        public static TResult GetValueOrDefault<TKey, TValue, TResult>(this IDictionary<TKey, TValue> dict, TKey key,
            Func<TValue, TResult> selector, TResult defaultValue = default(TResult))
        {
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            TValue value;
            return dict.TryGetValue(key, out value) ? selector(value) : defaultValue;
        }

        public static TResult GetValueOrDefault<TKey, TValue, TResult>(this IDictionary<TKey, TValue> dict, TKey key,
            Func<TValue, TResult> selector, Func<TResult> defaultValueFactory)
        {
            if (selector == null) throw new ArgumentNullException(nameof(selector));
            if (defaultValueFactory == null) throw new ArgumentNullException(nameof(defaultValueFactory));

            TValue value;
            return dict.TryGetValue(key, out value) ? selector(value) : defaultValueFactory();
        }

        public static TValue? GetValueOrNull<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key)
            where TValue : struct
        {
            if (dict == null) throw new ArgumentNullException(nameof(dict));

            TValue value;
            return dict.TryGetValue(key, out value) ? (TValue?)value : null;
        }

        #endregion

        #region get and/or set value

        /// <summary>
        /// return old value and set to new value.
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dict"></param>
        /// <param name="key"></param>
        /// <param name="newValue"></param>
        /// <param name="defaultReturnValue"></param>
        /// <returns></returns>
        public static TValue GetAndSetValue<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key,
            TValue newValue, TValue defaultReturnValue = default(TValue))
        {
            var ret = dict.GetValueOrDefault(key, defaultReturnValue);
            dict[key] = newValue;
            return ret;
        }


        public static TValue GetOrSetValue<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key)
            where TValue : new()
        {
            if (dict == null) throw new ArgumentNullException(nameof(dict));

            TValue r;

            if (!dict.TryGetValue(key, out r))
            {
                dict.Add(key, r = new TValue());
            }

            return r;
        }

        public static TValue GetOrSetValue<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key,
            TValue value)
        {
            if (dict == null) throw new ArgumentNullException(nameof(dict));

            TValue r;

            if (!dict.TryGetValue(key, out r))
            {
                dict.Add(key, r = value);
            }

            return r;
        }

        public static TValue GetOrSetValue<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key,
            Func<TValue> valueFactory)
        {
            if (dict == null) throw new ArgumentNullException(nameof(dict));
            if (valueFactory == null) throw new ArgumentNullException(nameof(valueFactory));

            TValue r;

            if (!dict.TryGetValue(key, out r))
            {
                dict.Add(key, r = valueFactory());
            }

            return r;
        }

        #endregion

        public static int RemoveKeyRange<TKey, TValue>(this IDictionary<TKey, TValue> obj, IEnumerable<TKey> keys)
            => keys.Count(obj.Remove);
    }
}
