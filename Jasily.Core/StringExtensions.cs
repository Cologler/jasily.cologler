using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace System
{
    public static class StringExtensions
    {
        #region byte[]

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

        #endregion

        #region is

        /// <summary>
        /// return String.IsNullOrEmpty(text)
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this string text)
        {
            return String.IsNullOrEmpty(text);
        }

        /// <summary>
        /// return String.IsNullOrWhiteSpace(text)
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static bool IsNullOrWhiteSpace(this string text)
        {
            return String.IsNullOrWhiteSpace(text);
        }

        #endregion

        #region throw

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

        #endregion

        #region split

        public static string[] Split(this string text, string separator, StringSplitOptions options = StringSplitOptions.None)
        {
            return text.Split(new[] { separator }, options);
        }

        public static string[] Split(this string text, string separator, int count, StringSplitOptions options = StringSplitOptions.None)
        {
            return text.Split(new [] { separator }, count, options);
        }

        #endregion

        #region format

        public static string Format(this string format, params object[] args)
        {
            return String.Format(format, args);
        }

        public static string Format(this string format, IFormatProvider provider, params object[] args)
        {
            return String.Format(format, provider, args);
        }

        #endregion

        #region parse

        public static int? TryParseInt32(string s)
        {
            int n;
            return Int32.TryParse(s, out n) ? (int?) n : null;
        }
        public static int? TryParseInt32(string s, NumberStyles style, IFormatProvider provider)
        {
            int n;
            return Int32.TryParse(s, style, provider, out n) ? (int?)n : null;
        }

        public static long? TryParseInt64(string s)
        {
            long n;
            return long.TryParse(s, out n) ? (long?)n : null;
        }
        public static long? TryParseInt64(string s, NumberStyles style, IFormatProvider provider)
        {
            long n;
            return long.TryParse(s, style, provider, out n) ? (long?)n : null;
        }

        public static double? TryParseDouble(string s)
        {
            double n;
            return double.TryParse(s, out n) ? (long?)n : null;
        }
        public static double? TryParseDouble(string s, NumberStyles style, IFormatProvider provider)
        {
            double n;
            return double.TryParse(s, style, provider, out n) ? (double?)n : null;
        }

        #endregion

        #region other

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
        /// use spliter to split text. default value was '\r\n'
        /// </summary>
        /// <param name="text"></param>
        /// <param name="spliter"></param>
        /// <returns></returns>
        public static IEnumerable<string> AsLines(this string text, string spliter = "\r\n")
        {
            return text == null ? null : text.Split(new string[] {spliter}, StringSplitOptions.None);
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

        /// <summary>
        /// return first line from text
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string FirstLine(this string text)
        {
            if (text == null)
                return null;

            var index = text.IndexOf('\n');

            if (index == -1)
            {
                return text;
            }
            else
            {
                if (index > 0 && text[index - 1] == '\r')
                {
                    return text.Substring(0, index - 1);
                }
                else
                {
                    return text.Substring(0, index);
                }
            }
        }

        #endregion
    }
}
