
using System.Collections.ObjectModel;
using System.Linq;
using JetBrains.Annotations;

namespace System.Collections.Generic
{
    public static class DictionaryExtensions
    {
        /* desciption:
         * 1. key don't need check "if (key == null) throw;" because .TryGetValue() will check it.
         */

        #region IDictionary

        #region get value or default

        public static TValue GetValueOrDefault<TKey, TValue>([NotNull] this IDictionary<TKey, TValue> dict,
            [NotNull] TKey key, TValue defaultValue = default(TValue))
        {
            if (dict == null) throw new ArgumentNullException(nameof(dict));

            TValue value;
            return dict.TryGetValue(key, out value) ? value : defaultValue;
        }

        public static TValue GetValueOrDefault<TKey, TValue>([NotNull] this IDictionary<TKey, TValue> dict,
            [NotNull] TKey key, [NotNull] Func<TValue> defaultValueFactory)
        {
            if (dict == null) throw new ArgumentNullException(nameof(dict));
            if (defaultValueFactory == null) throw new ArgumentNullException(nameof(defaultValueFactory));

            TValue value;
            return dict.TryGetValue(key, out value) ? value : defaultValueFactory();
        }

        public static TResult GetValueOrDefault<TKey, TValue, TResult>([NotNull] this IDictionary<TKey, TValue> dict,
            [NotNull] TKey key, [NotNull] Func<TValue, TResult> selector, TResult defaultValue = default(TResult))
        {
            if (dict == null) throw new ArgumentNullException(nameof(dict));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            TValue value;
            return dict.TryGetValue(key, out value) ? selector(value) : defaultValue;
        }

        public static TResult GetValueOrDefault<TKey, TValue, TResult>([NotNull] this IDictionary<TKey, TValue> dict,
            [NotNull] TKey key, [NotNull] Func<TValue, TResult> selector, [NotNull] Func<TResult> defaultValueFactory)
        {
            if (dict == null) throw new ArgumentNullException(nameof(dict));
            if (selector == null) throw new ArgumentNullException(nameof(selector));
            if (defaultValueFactory == null) throw new ArgumentNullException(nameof(defaultValueFactory));

            TValue value;
            return dict.TryGetValue(key, out value) ? selector(value) : defaultValueFactory();
        }

        public static TValue? GetValueOrNull<TKey, TValue>([NotNull] this IDictionary<TKey, TValue> dict,
            [NotNull] TKey key)
            where TValue : struct
        {
            if (dict == null) throw new ArgumentNullException(nameof(dict));

            TValue value;
            return dict.TryGetValue(key, out value) ? (TValue?)value : null;
        }

        #endregion

        #region get and/or set value

        /// <summary>
        /// return old value and set to new value. (if key not found will throw a KeyNotFoundException)
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dict"></param>
        /// <param name="key"></param>
        /// <param name="newValue"></param>
        /// <exception cref="KeyNotFoundException"></exception>
        /// <returns></returns>
        public static TValue GetAndSetValue<TKey, TValue>([NotNull] this IDictionary<TKey, TValue> dict,
            [NotNull] TKey key, TValue newValue)
        {
            if (dict == null) throw new ArgumentNullException(nameof(dict));

            var ret = dict[key];
            dict[key] = newValue;
            return ret;
        }

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
        public static TValue GetAndSetValue<TKey, TValue>([NotNull] this IDictionary<TKey, TValue> dict,
            [NotNull] TKey key, TValue newValue, TValue defaultReturnValue)
        {
            if (dict == null) throw new ArgumentNullException(nameof(dict));

            var ret = dict.GetValueOrDefault(key, defaultReturnValue);
            dict[key] = newValue;
            return ret;
        }

        public static TValue GetOrCreateValue<TKey, TValue>([NotNull] this IDictionary<TKey, TValue> dict,
            [NotNull] TKey key) where TValue : new()
        {
            if (dict == null) throw new ArgumentNullException(nameof(dict));

            TValue r;

            if (!dict.TryGetValue(key, out r))
            {
                dict.Add(key, r = new TValue());
            }

            return r;
        }

        public static TValue GetOrSetValue<TKey, TValue>([NotNull] this IDictionary<TKey, TValue> dict,
            [NotNull] TKey key, TValue value = default(TValue))
        {
            if (dict == null) throw new ArgumentNullException(nameof(dict));

            TValue r;

            if (!dict.TryGetValue(key, out r))
            {
                dict.Add(key, r = value);
            }

            return r;
        }

        public static TValue GetOrSetValue<TKey, TValue>([NotNull] this IDictionary<TKey, TValue> dict,
            [NotNull] TKey key, [NotNull] Func<TValue> valueFactory)
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

        public static int RemoveKeyRange<TKey, TValue>([NotNull] this IDictionary<TKey, TValue> dict,
            [NotNull][ItemNotNull] IEnumerable<TKey> keys) => keys.Count(dict.Remove);

        public static IReadOnlyDictionary<TKey, TValue> AsReadOnly<TKey, TValue>(
            [NotNull] this IDictionary<TKey, TValue> dict)
        {
            if (dict == null) throw new ArgumentNullException(nameof(dict));
            return new ReadOnlyDictionary<TKey, TValue>(dict);
        }

        public static void ValueMoveNext<TKey, TValue>([NotNull] this IDictionary<TKey, TValue> dict,
            [NotNull] TKey key, TValue initValue, [NotNull] Func<TValue, TValue> getNextFunc)
        {
            if (dict == null) throw new ArgumentNullException(nameof(dict));
            if (getNextFunc == null) throw new ArgumentNullException(nameof(getNextFunc));

            TValue value;
            if (!dict.TryGetValue(key, out value))
            {
                dict[key] = initValue;
            }
            else
            {
                dict[key] = getNextFunc(value);
            }
        }

        #region IGetKey

        public static void Add<TKey, TValue>([NotNull] this IDictionary<TKey, TValue> dict, [NotNull] TValue item)
            where TValue : IGetKey<TKey>
        {
            if (dict == null) throw new ArgumentNullException(nameof(dict));
            if (item == null) throw new ArgumentNullException(nameof(item));
            var key = item.GetKey();
            if (key == null) throw new ArgumentNullException(nameof(key));

            dict.Add(key, item);
        }

        public static void AddRange<TKey, TValue>([NotNull] this IDictionary<TKey, TValue> dict,
            [NotNull] IEnumerable<TValue> items)
            where TValue : IGetKey<TKey>
        {
            if (dict == null) throw new ArgumentNullException(nameof(dict));
            if (items == null) throw new ArgumentNullException(nameof(items));

            var itemArray = items.ToArray();
            var keyArray = itemArray.Select(z => z.GetKey()).ToArray();
            if (keyArray.Any(z => z == null)) throw new ArgumentException("GetKey() can not return null.");
            for (var i = 0; i < itemArray.Length; i++) dict.Add(keyArray[i], itemArray[i]);
        }

        public static void Set<TKey, TValue>([NotNull] this IDictionary<TKey, TValue> dict, [NotNull] TValue item)
            where TValue : IGetKey<TKey>
        {
            if (dict == null) throw new ArgumentNullException(nameof(dict));
            if (item == null) throw new ArgumentNullException(nameof(item));
            var key = item.GetKey();
            if (key == null) throw new ArgumentNullException(nameof(key));

            dict[key] = item;
        }

        #endregion

        #endregion
    }
}
