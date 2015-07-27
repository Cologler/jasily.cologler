
using System.Linq;

namespace System.Collections.Generic
{
    public static class IDictionaryExtensions
    {
        public static int RemoveRange<TKey, TValue>(this IDictionary<TKey, TValue> obj, IEnumerable<TKey> keys)
        {
            return keys.Count(obj.Remove);
        }

        /// <summary>
        /// return last value then set to @value.
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="obj"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="defaultLastValue"></param>
        /// <returns></returns>
        public static TValue GetLastAndSetValue<TKey, TValue>(this IDictionary<TKey, TValue> obj, TKey key,
            TValue value, TValue defaultLastValue = default(TValue))
        {
            try
            {
                return TryGetValueExtensions.GetValueOrDefault(obj.TryGetValue, key, defaultLastValue);
            }
            finally
            {
                obj[key] = value;
            }
        }

        /// <summary>
        /// try get value. if @key was not exists, set and return @value.
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="obj"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
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

        /// <summary>
        /// try get value. if @key was not exists, set and return @valueFunc().
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="obj"></param>
        /// <param name="key"></param>
        /// <param name="valueFunc"></param>
        /// <returns></returns>
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
    }
}
