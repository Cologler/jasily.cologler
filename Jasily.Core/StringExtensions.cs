using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace System
{
    public static class StringExtensions
    {
        #region encoding & decoding

        /// <summary>
        /// get bytes using special encoding
        /// </summary>
        /// <param name="text"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static byte[] GetBytes(this string text, Encoding encoding = null)
            => (encoding ?? Encoding.UTF8).GetBytes(text);

        public static string UrlEncode(this string str) => Net.WebUtility.UrlEncode(str);

        public static string UrlDecode(this string str) => Net.WebUtility.UrlDecode(str);

        #endregion

        #region is

        /// <summary>
        /// return String.IsNullOrEmpty(text)
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this string text) => string.IsNullOrEmpty(text);

        /// <summary>
        /// return String.IsNullOrWhiteSpace(text)
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static bool IsNullOrWhiteSpace(this string text) => string.IsNullOrWhiteSpace(text);

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
            if (string.IsNullOrEmpty(text))
            {
                if (message == null)
                    throw new ArgumentException(paramName);
                else
                    throw new ArgumentException(paramName, message);
            }

            return text;
        }

        /// <summary>
        /// throw if current text is null or empty.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="paramName"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static string ThrowIfNullOrWhiteSpace(this string text, string paramName, string message = null)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                if (message == null)
                    throw new ArgumentException(paramName);
                else
                    throw new ArgumentException(paramName, message);
            }

            return text;
        }

        #endregion

        #region parse

        public static int? TryParseInt32(string s)
        {
            int n;
            return int.TryParse(s, out n) ? (int?)n : null;
        }
        public static int? TryParseInt32(string s, NumberStyles style, IFormatProvider provider)
        {
            int n;
            return int.TryParse(s, style, provider, out n) ? (int?)n : null;
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

        #region repeat

        /// <summary>
        /// repeat this char.   
        /// just like ( string * int ) in python.
        /// </summary>
        /// <param name="ch"></param>
        /// <param name="count"></param>
        /// <exception cref="System.ArgumentOutOfRangeException">count &lt; 0</exception>
        /// <returns></returns>
        public static string Repeat(this char ch, int count) => new string(ch, count);

        /// <summary>
        /// repeat this string. 
        /// just like ( string * int ) in python
        /// </summary>
        /// <param name="str"></param>
        /// <param name="count"></param>
        /// <exception cref="System.ArgumentOutOfRangeException">count &lt; 0</exception>
        /// <returns></returns>
        public static string Repeat(this string str, int count)
            => str == null ? null : string.Concat(Enumerable.Repeat(str, count));

        #endregion

        public static bool Contains(this string str, string value, StringComparison comparisonType)
            => str.IndexOf(value, comparisonType) > -1;

        #region split & join

        public static string[] Split(this string text, string separator, StringSplitOptions options = StringSplitOptions.None)
            => text.Split(new[] { separator }, options);

        public static string[] Split(this string text, string separator, int count, StringSplitOptions options = StringSplitOptions.None)
            => text.Split(new[] { separator }, count, options);

        public static string JoinWith(this IEnumerable<string> texts, string spliter)
            => string.Join(spliter, texts);

        /// <summary>
        /// use '\r\n' or '\n' to split text.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static IEnumerable<string> AsLines(this string text)
            => text?.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);

        /// <summary>
        /// use Environment.NewLine to join texts.
        /// </summary>
        /// <param name="texts"></param>
        /// <returns></returns>
        public static string AsLines(this IEnumerable<string> texts)
            => string.Join(Environment.NewLine, texts);

        #endregion

        #region other

        /// <summary>
        /// return first line from text
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string FirstLine(this string text)
        {
            if (text == null) return null;
            var index = text.IndexOf('\n');
            return index == -1 ? text : text.Substring(0, index > 0 && text[index - 1] == '\r' ? index - 1 : index);
        }

        #endregion

        #region

        public static string TrimStart(this string str, params string[] trimStrings)
        {
            if (str == null) throw new ArgumentNullException(nameof(str));

            if (trimStrings == null || trimStrings.Length == 0)
                return str;

            if (trimStrings.Any(z => z.IsNullOrEmpty()))
                throw new ArgumentException();

            string start;
            while ((start = trimStrings.FirstOrDefault(z => str.StartsWith(z))) != null)
            {
                str = str.Substring(start.Length);
            }
            return str;
        }

        #endregion

        #region get string

        public static string GetString(this char[] array) => new string(array);

        public static string GetString(this IEnumerable<char> array) => new string(array.ToArray());

        #endregion

        #region common part

        public static string CommonStart(this string[] source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (source.Length == 0) return string.Empty;
            if (source.Length == 1) return source[0];

            var exp = source[0];
            var end = source.Min(z => z.Length); // length
            foreach (var item in source.Skip(1))
            {
                for (var i = 0; i < end; i++)
                {
                    if (item[i] != exp[i])
                    {
                        end = i;
                        break;
                    }
                }
            }
            return exp.Substring(0, end);
        }

        public static string CommonEnd(this string[] source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (source.Length == 0) return string.Empty;
            if (source.Length == 1) return source[0];

            var exp = source[0];
            var len = source.Select(z => z.Length).Min();
            foreach (var item in source.Skip(1))
            {
                for (var i = 0; i < len; i++)
                {
                    if (exp[exp.Length - i - 1] != item[item.Length - i - 1])
                    {
                        len = i;
                        break;
                    }
                }
            }
            return exp.Substring(exp.Length - len);
        }

        #endregion

        #region after first and last

        public static string AfterFirst(this string str, string spliter)
        {
            if (str == null) throw new ArgumentNullException(nameof(str));
            if (string.IsNullOrEmpty(spliter)) throw new ArgumentNullException(nameof(spliter));

            var index = str.IndexOf(spliter, StringComparison.Ordinal);
            return index < 1 ? str : str.Substring(index + 1);
        }
        public static string AfterFirst(this string str, char spliter)
        {
            if (str == null) throw new ArgumentNullException(nameof(str));

            var index = str.IndexOf(spliter);
            return index < 1 ? str : str.Substring(index + 1);
        }
        public static string AfterFirst(this string str, params char[] spliter)
        {
            if (str == null) throw new ArgumentNullException(nameof(str));

            var index = str.LastIndexOfAny(spliter);
            return index < 1 ? str : str.Substring(index + 1);
        }

        public static string AfterLast(this string str, string spliter)
        {
            if (str == null) throw new ArgumentNullException(nameof(str));
            if (string.IsNullOrEmpty(spliter)) throw new ArgumentNullException(nameof(spliter));

            var index = str.LastIndexOf(spliter, StringComparison.Ordinal);
            return index < 1 ? str : str.Substring(index + 1);
        }
        public static string AfterLast(this string str, char spliter)
        {
            if (str == null) throw new ArgumentNullException(nameof(str));

            var index = str.LastIndexOf(spliter);
            return index < 1 ? str : str.Substring(index + 1);
        }
        public static string AfterLast(this string str, params char[] spliter)
        {
            if (str == null) throw new ArgumentNullException(nameof(str));

            var index = str.LastIndexOfAny(spliter);
            return index < 1 ? str : str.Substring(index + 1);
        }

        #endregion

        public static string Childs(this string str, int? firstIndex = null, int? lastIndex = null)
        {
            if (str == null) throw new ArgumentNullException(nameof(str));
            if (str.Length == 0) return string.Empty;

            return JasilyStringChild.Instance.Result(str, firstIndex, lastIndex);
        }

        private class JasilyStringChild : IChild<string, string>
        {
            public static readonly JasilyStringChild Instance = new JasilyStringChild();

            public int Count(string source) => source.Length;

            public string Empty => string.Empty;

            public string GetChilds(string source, int firstIndex, int lastIndex)
                => source.Substring(firstIndex, lastIndex - firstIndex + 1);
        }
    }

    internal interface IChild<TSource, TResult>
    {
        int Count(TSource source);

        TResult Empty { get; }

        TResult GetChilds(TSource source, int firstIndex, int lastIndex);
    }

    internal static class IChildExtensions
    {
        public static TResult Result<TSource, TResult>(this IChild<TSource, TResult> child,
            TSource source, int? firstIndex, int? lastIndex)
        {
            var length = child.Count(source);
            if (length == 0) return child.Empty;

            var left = firstIndex ?? 0;
            if (left > length) return child.Empty;
            if (left < 0) left = length + left;
            if (left < 0) left = 0;

            var right = lastIndex ?? length - 1;
            if (right >= length) return child.Empty;
            if (right < 0) right = length + right;
            if (right < 0) right = 0;
            if (left >= right) return child.Empty;

            return child.GetChilds(source, left, right);
        }
    }
}
