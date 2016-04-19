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
    }
}