
using JetBrains.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;
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

        /// <summary>
        /// 为链式编程提供支持
        /// </summary>
        /// <typeparam name="TIn"></typeparam>
        /// <typeparam name="TOut"></typeparam>
        /// <param name="obj"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static TOut CastWith<TIn, TOut>(this TIn obj, Func<TIn, TOut> selector)
        {
            Assert(selector != null);

            return selector(obj);
        }

        /// <summary>
        /// return a single item array : new[] { obj }.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T[] IntoArray<T>(this T obj) => new[] { obj };

        #region is

        /// <summary>
        /// return self in [lower, upper) 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="self"></param>
        /// <param name="lower"></param>
        /// <param name="upper"></param>
        /// <returns></returns>
        public static bool IsBetween<T>(this T self, T lower, T upper) where T : IComparable<T>
            => self.CompareTo(lower) >= 0 && self.CompareTo(upper) < 0;

        #endregion

        public static NameValuePair<string, T> WithName<T>(this T obj, string name) => new NameValuePair<string, T>(name, obj);

        #region try

        public static T Try<TSource, T>(this TSource obj, [NotNull] Func<T> func, [NotNull] Func<T, bool> match, int maxTryCount)
        {
            if (func == null) throw new ArgumentNullException(nameof(func));
            if (match == null) throw new ArgumentNullException(nameof(match));
            if (maxTryCount < 1) throw new ArgumentOutOfRangeException();

            T r;
            do
            {
                r = func();
                if (match(r)) return r;
                maxTryCount--;
            } while (maxTryCount > 0);
            return r;
        }

        public static async Task<T> TryAsync<TSource, T>(this TSource obj, [NotNull] Func<Task<T>> func, [NotNull] Func<T, bool> match, int maxTryCount)
        {
            if (func == null) throw new ArgumentNullException(nameof(func));
            if (match == null) throw new ArgumentNullException(nameof(match));
            if (maxTryCount < 1) throw new ArgumentOutOfRangeException();

            T r;
            do
            {
                r = await func();
                if (match(r)) return r;
                maxTryCount--;
            } while (maxTryCount > 0);
            return r;
        }

        #endregion
    }
}
