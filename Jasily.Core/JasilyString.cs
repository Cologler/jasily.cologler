using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System
{
    public static class JasilyString
    {
        /// <summary>
        /// get bytes using special encoding
        /// </summary>
        /// <param name="text"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static byte[] GetBytes(this string text, Encoding encoding)
        {
            return encoding.GetBytes(text);
        }

        /// <summary>
        /// get bytes using utf-8
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static byte[] GetBytes(this string text)
        {
            return text.GetBytes(Encoding.UTF8);
        }

        /// <summary>
        /// throw if current text is null or empty.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="paramName"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static string ThrowIfNullOrEmpty(this string text, string paramName, string message = null)
        {
            if (text == null)
            {
                if (message == null)
                    throw new ArgumentNullException(paramName);
                else
                    throw new ArgumentNullException(paramName, message);
            }
            else if (text.Length == 0)
            {
                if (message == null)
                    throw new ArgumentException(paramName);
                else
                    throw new ArgumentException(paramName, message);
            }


            return text;
        }

        /// <summary>
        /// use spliter to join texts. default value was '\r\n'
        /// </summary>
        /// <param name="texts"></param>
        /// <param name="spliter"></param>
        /// <returns></returns>
        public static string AsLines(this IEnumerable<string> texts, string spliter = "\r\n")
        {
            return String.Join(spliter, texts);
        }

        /// <summary>
        /// repeat this like ( string * int ) in python
        /// </summary>
        /// <param name="str"></param>
        /// <param name="count"></param>
        /// <exception cref="System.ArgumentOutOfRangeException">count &lt; 0</exception>
        /// <returns></returns>
        public static string Repeat(this string str, int count)
        {
            return str == null ? null : string.Concat(Enumerable.Repeat(str, count));
        }

        public static string FirstLine(this string source)
        {
            if (source == null)
                return null;

            var index = source.IndexOf('\n');

            if (index == -1)
            {
                return source;
            }
            else
            {
                if (index > 0 && source[index - 1] == '\r')
                {
                    return source.Substring(0, index - 1);
                }
                else
                {
                    return source.Substring(0, index);
                }
            }
        }
    }
}
