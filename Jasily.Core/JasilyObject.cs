
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
        /// performance test can see: http://www.evernote.com/l/ALKIesUPaCJEv6WcQs1MqMeZN8hcMympy1U/, fast than 'as'
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T As<T>(this T obj)
        {
            return obj;
        }
    }
}
