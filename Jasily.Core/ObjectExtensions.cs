using System.Collections.Generic;
using System.Linq.Expressions;
using JetBrains.Annotations;

namespace System
{
    public static class ObjectExtensions
    {
        public static bool NormalEquals<T>(this T obj, T other) => EqualityComparer<T>.Default.Equals(obj, other);

        #region c# 6.0 null condition extensions

        /// <summary>
        /// if obj is special type, do action().
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="action"></param>
        public static void IfType<T>(this object obj, [NotNull] Action<T> action)
            where T : class
        {
            if (action == null) throw new ArgumentNullException(nameof(action));
            var t = obj as T;
            if (t != null) action(t);
        }

        /// <summary>
        /// if obj is special type, return selector(obj), else return def.
        /// </summary>
        /// <typeparam name="TIn"></typeparam>
        /// <typeparam name="TOut"></typeparam>
        /// <param name="obj"></param>
        /// <param name="selector"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public static TOut IfType<TIn, TOut>(this object obj, [NotNull] Func<TIn, TOut> selector,
            TOut def = default(TOut))
            where TIn : class
        {
            if (selector == null) throw new ArgumentNullException(nameof(selector));
            var t = obj as TIn;
            return t == null ? def : selector(t);
        }

        /// <summary>
        /// if obj is special type, return selector(obj), else return def().
        /// </summary>
        /// <typeparam name="TIn"></typeparam>
        /// <typeparam name="TOut"></typeparam>
        /// <param name="obj"></param>
        /// <param name="selector"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public static TOut IfType<TIn, TOut>(this object obj, [NotNull] Func<TIn, TOut> selector,
            [NotNull] Func<TOut> def)
            where TIn : class
        {
            if (selector == null) throw new ArgumentNullException(nameof(selector));
            if (def == null) throw new ArgumentNullException(nameof(def));
            var t = obj as TIn;
            return t == null ? def() : selector(t);
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
        public static TOut CastWith<TIn, TOut>(this TIn obj, Func<TIn, TOut> selector) => selector(obj);

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

        #region cast

        /// <summary>
        /// cast without boxing.
        /// alse see: https://stackoverflow.com/questions/1189144/c-sharp-non-boxing-conversion-of-generic-enum-to-int
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static Cast<T> Casting<T>(this T source) => new Cast<T>(source);

        public struct Cast<T>
        {
            private readonly T source;

            public Cast(T source)
            {
                this.source = source;
            }

            public TDest To<TDest>() => CheckedConverter<TDest>.Convert(this.source);

            public TDest UncheckedTo<TDest>() => UncheckedConverter<TDest>.Convert(this.source);

            private static class CheckedConverter<TDest>
            {
                internal static readonly Func<T, TDest> Convert;

                static CheckedConverter()
                {
                    var p = Expression.Parameter(typeof(T));
                    var c = Expression.ConvertChecked(p, typeof(TDest));
                    Convert = Expression.Lambda<Func<T, TDest>>(c, p).Compile();
                }
            }

            private static class UncheckedConverter<TDest>
            {
                internal static readonly Func<T, TDest> Convert;

                static UncheckedConverter()
                {
                    var p = Expression.Parameter(typeof(T));
                    var c = Expression.Convert(p, typeof(TDest));
                    Convert = Expression.Lambda<Func<T, TDest>>(c, p).Compile();
                }
            }
        }

        #endregion
    }
}
