
namespace System
{
    public static class JasilyObject
    {
        /// <summary>
        /// if both are not null, using Equals() to check if they equals.
        /// <para/>
        /// both are null, return true.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="other"></param>
        /// <returns></returns>
        public static bool NormalEquals<T>(this T obj, T other)
        {
            return (obj == null && other == null) || (obj != null && obj.Equals(other));
        }

        /// <summary>
        /// if obj is special type, action
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="action"></param>
        public static void TypeOf<T>(this object obj, Action<T> action)
            where T : class
        {
            var t = obj as T;

            if (t != null)
                action(t);
        }
        /// <summary>
        /// if obj is special type, selector, or return def
        /// </summary>
        /// <typeparam name="TIn"></typeparam>
        /// <typeparam name="TOut"></typeparam>
        /// <param name="obj"></param>
        /// <param name="selector"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public static TOut TypeOf<TIn, TOut>(this object obj, Func<TIn, TOut> selector, TOut def = default(TOut))
            where TIn : class
        {
            var t = obj as TIn;

            if (t != null)
                return selector(t);
            else
                return def;
        }
        /// <summary>
        /// if obj is special type, selector, or call def()
        /// </summary>
        /// <typeparam name="TIn"></typeparam>
        /// <typeparam name="TOut"></typeparam>
        /// <param name="obj"></param>
        /// <param name="selector"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public static TOut TypeOf<TIn, TOut>(this object obj, Func<TIn, TOut> selector, Func<TOut> def)
            where TIn : class
        {
            var t = obj as TIn;

            if (t != null)
                return selector(t);
            else
                return def();
        }
    }
}
