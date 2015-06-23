
using System.Runtime.CompilerServices;

namespace System
{
    public static class JasilyObjectHelper
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
        /// performance test can see: http://www.evernote.com/l/ALKIesUPaCJEv6WcQs1MqMeZN8hcMympy1U/, fast than 'as'
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [Obsolete("if not inline, should slow than 'as', wtf.")]
        public static T As<T>(this T obj)
        {
            return obj;
        }

        /// <summary>
        /// if obj is special type, action
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="action"></param>
        public static void IfType<T>(this object obj, Action<T> action)
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
        public static TOut IfType<TIn, TOut>(this object obj, Func<TIn, TOut> selector, TOut def = default(TOut))
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
        public static TOut IfType<TIn, TOut>(this object obj, Func<TIn, TOut> selector, Func<TOut> def)
            where TIn : class
        {
            var t = obj as TIn;

            if (t != null)
                return selector(t);
            else
                return def();
        }

        public static T Cast<T>(this object obj)
        {
            return (T) obj;
        }
    }
}
