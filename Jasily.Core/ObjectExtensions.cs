
using System.Runtime.CompilerServices;
using static System.Diagnostics.Debug;

namespace System
{
    public static class ObjectExtensions
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
            if (ReferenceEquals(obj, other)) return true;
            if (ReferenceEquals(obj, null) || ReferenceEquals(other, null)) return false;
            
            return (obj as IEquatable<T>)?.Equals(other) ?? obj.Equals(other);
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

            if (t != null) action(t);
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

            return t == null ? def : selector(t);
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

            return t == null ? def() : selector(t);
        }

        #region type convert

        /// <summary>
        /// if obj is type, return the obj self, else return a new object.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool TryTypeConvert(this object obj, Type type, out object dest)
        {
            if (ReferenceEquals(obj, null) || obj.GetType() == type)
            {
                dest = obj;
                return true;
            }

            switch (obj.GetType().FullName)
            {
                case "System.String":
                    return TryTypeConvert((string)obj, type, out dest);

                case "System.Int32":
                    return TryTypeConvert((int)obj, type, out dest);

                case "System.Int64":
                    return TryTypeConvert((long)obj, type, out dest);

                default:
                    dest = null;
                    return false;
            }
        }

        public static bool TryTypeConvert(this string obj, Type type, out object dest)
        {
            if (ReferenceEquals(obj, null) || obj.GetType() == type)
            {
                dest = obj;
                return true;
            }

            switch (type.Name)
            {
                case "System.Int32":
                    dest = Int32.Parse(obj);
                    break;

                case "System.Int64":
                    dest = Int64.Parse(obj);
                    break;

                default:
                    dest = null;
                    return false;
            }

            return true;
        }

        public static bool TryTypeConvert(this int obj, Type type, out object dest)
        {
            if (obj.GetType() == type)
            {
                dest = obj;
                return true;
            }

            switch (type.Name)
            {
                case "System.String":
                    dest = obj.ToString();
                    break;

                case "System.Int64":
                    dest = Convert.ToInt64(obj);
                    break;

                default:
                    dest = null;
                    return false;
            }

            return true;
        }

        public static bool TryTypeConvert(this long obj, Type type, out object dest)
        {
            if (obj.GetType() == type)
            {
                dest = obj;
                return true;
            }

            switch (type.Name)
            {
                case "System.String":
                    dest = obj.ToString();
                    break;

                case "System.Int32":
                    dest = Convert.ToInt32(obj);
                    break;

                default:
                    dest = null;
                    return false;
            }

            return true;
        }

        #endregion

        public static TOut CastWith<TIn, TOut>(this TIn obj, Func<TIn, TOut> selector)
        {
            Assert(selector != null);

            return selector(obj);
        }

        /// <summary>
        /// 拆箱操作 @_@
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T? TryCast<T>(this object obj)
            where T : struct
        {
            return obj is T ? (T)obj : (T?)null;
        }
    }
}
