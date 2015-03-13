
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
    }
}
