
namespace System
{
    public static class JasilyThrow
    {
        /// <summary>
        /// throw if current object is null.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="paramName"></param>
        /// <param name="message"></param>
        /// <exception cref="System.ArgumentNullException">throw if current object is null.</exception>
        /// <returns></returns>
        public static T ThrowIfNull<T>(this T obj, string paramName, string message = null)
        {
            if (obj == null)
            {
                if (message == null)
                    throw new ArgumentNullException(paramName);
                else
                    throw new ArgumentNullException(paramName, message);
            }

            return obj;
        }
    }
}
