using System.Collections.Generic;

namespace System
{
    public static class EnumExtensions
    {
        /// <summary>
        /// for enum to get like: "DayOfWeek.Monday"
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="spliter"></param>
        /// <returns></returns>
        public static string ToFullString<T>(this T value, string spliter = ".") where T : struct
            => string.Concat(value.GetType().Name, spliter ?? ".", value.ToString());

        /// <summary>
        /// compare two enum without boxing
        /// alse see: https://stackoverflow.com/questions/35442450/how-to-compare-system-enum-to-enum-implementation-without-boxing
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public static bool Equals<TEnum>(this Enum first, TEnum second) where TEnum : struct
        {
            var asEnumType = first as TEnum?;
            return asEnumType.HasValue && EqualityComparer<TEnum>.Default.Equals(asEnumType.Value, second);
        }

        /// <summary>
        /// for enum (with boxing).
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public static bool HasFlag2<TEnum>(this TEnum first, TEnum second) where TEnum : struct
        {
            var o = second.Casting().UncheckedTo<ulong>();
            return (first.Casting().UncheckedTo<ulong>() & o) == o;
        }
    }
}