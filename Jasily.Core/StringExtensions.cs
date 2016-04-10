using JetBrains.Annotations;
using System.Collections.Generic;
using System.Diagnostics;
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
        public static string Repeat(this char ch, int count) => count == 0 ? string.Empty : new string(ch, count);

        /// <summary>
        /// repeat this string. 
        /// just like ( string * int ) in python
        /// </summary>
        /// <param name="str"></param>
        /// <param name="count"></param>
        /// <exception cref="System.ArgumentOutOfRangeException">count &lt; 0</exception>
        /// <returns></returns>
        public static string Repeat(this string str, int count)
            => str == null ? null : (count == 0 ? string.Empty : string.Concat(Enumerable.Repeat(str, count)));

        #endregion

        public static string Take([NotNull] this string str, int count)
        {
            if (str == null) throw new ArgumentNullException(nameof(str));
            if (count < 0) throw new ArgumentOutOfRangeException();
            if (count > str.Length) return str;
            return str.Substring(0, count);
        }

        #region replace

        private static IEnumerable<string> AppendStart(IEnumerable<string> source, string next)
        {
            Debug.Assert(source != null);
            yield return next;
            foreach (var item in source) yield return item;
        }

        private static IEnumerable<string> AppendEnd(IEnumerable<string> source, string next)
        {
            Debug.Assert(source != null);
            foreach (var item in source) yield return item;
            yield return next;
        }

        public static string Replace([NotNull] this string str, [NotNull] string oldValue,
            [NotNull] string newValue, StringComparison comparisonType)
        {
            if (str == null) throw new ArgumentNullException(nameof(str));
            if (oldValue == null) throw new ArgumentNullException(nameof(oldValue));
            if (newValue.Length == 0) throw new ArgumentException("Argument is empty", nameof(oldValue));
            if (newValue == null) throw new ArgumentNullException(nameof(newValue));
            if (str.Length < oldValue.Length) return str;

            if (comparisonType == StringComparison.Ordinal) return str.Replace(oldValue, newValue);

            var sb = new StringBuilder(str.Length);
            var ptr = 0;
            while (true)
            {
                var index = str.IndexOf(oldValue, ptr, comparisonType);
                if (index < 0) return sb.Append(str, ptr).ToString();
                sb.Append(str, ptr, index - ptr);
                sb.Append(newValue);
                ptr = index + oldValue.Length;
            }
        }

        public static string ReplaceStart([NotNull] this string str, [NotNull] string oldValue,
            [NotNull] string newValue, StringComparison comparisonType = StringComparison.Ordinal)
        {
            if (str == null) throw new ArgumentNullException(nameof(str));
            if (oldValue == null) throw new ArgumentNullException(nameof(oldValue));
            if (oldValue.Length == 0) throw new ArgumentException("Argument is empty", nameof(oldValue));
            if (newValue == null) throw new ArgumentNullException(nameof(newValue));
            if (str.Length < oldValue.Length) return str;

            var ptr = 0;
            var count = 0;
            while (str.StartsWith(ptr, oldValue, comparisonType))
            {
                ptr += oldValue.Length;
                count++;
            }

            if (count == 0) return str;
            return string.Concat(AppendEnd(Enumerable.Repeat(newValue, count), str.Substring(ptr)));
        }

        public static string ReplaceEnd([NotNull] this string str, [NotNull] string oldValue,
            [NotNull] string newValue, StringComparison comparisonType = StringComparison.Ordinal)
        {
            if (str == null) throw new ArgumentNullException(nameof(str));
            if (oldValue == null) throw new ArgumentNullException(nameof(oldValue));
            if (newValue.Length == 0) throw new ArgumentException("Argument is empty", nameof(oldValue));
            if (newValue == null) throw new ArgumentNullException(nameof(newValue));
            if (str.Length < oldValue.Length) return str;

            var ptr = 0;
            var count = 0;
            while (str.EndsWith(str.Length - ptr, oldValue, comparisonType))
            {
                ptr += oldValue.Length;
                count++;
            }

            if (count == 0) return str;
            return string.Concat(AppendStart(Enumerable.Repeat(newValue, count), str.Take(str.Length - ptr)));
        }

        #endregion

        #region start or end

        public static bool StartsWith([NotNull] this string str, int startIndex, [NotNull] string value,
            StringComparison comparisonType = StringComparison.Ordinal)
        {
            if (str == null) throw new ArgumentNullException(nameof(str));
            if (value == null) throw new ArgumentNullException(nameof(value));
            if (value.Length == 0) return true;

            if (startIndex == 0) return str.StartsWith(value, comparisonType);

            return string.Compare(str, startIndex, value, 0, value.Length, comparisonType) == 0;
        }

        public static bool EndsWith([NotNull] this string str, int count, [NotNull] string value,
            StringComparison comparisonType = StringComparison.Ordinal)
        {
            if (str == null) throw new ArgumentNullException(nameof(str));
            if (value == null) throw new ArgumentNullException(nameof(value));
            if (count < 0 || count > str.Length) throw new ArgumentOutOfRangeException(nameof(count));
            if (value.Length == 0) return true;

            if (count == str.Length) return str.EndsWith(value, comparisonType);

            var startIndex = count - value.Length;
            if (startIndex < 0) return false;
            return string.Compare(str, startIndex, value, 0, value.Length, comparisonType) == 0;
        }

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
        /// <param name="options"></param>
        /// <returns></returns>
        public static string[] AsLines(this string text, StringSplitOptions options = StringSplitOptions.None)
            => text?.Split(new[] { "\r\n", "\n" }, options);

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

        #region trim

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

        #region after/before first and last

        public static string AfterFirst([NotNull] this string str, [NotNull] string spliter,
            StringComparison comparisonType = StringComparison.Ordinal)
        {
            if (str == null) throw new ArgumentNullException(nameof(str));
            if (string.IsNullOrEmpty(spliter)) throw new ArgumentException(nameof(spliter));

            var index = str.IndexOf(spliter, comparisonType);
            return index < 1 ? str : str.Substring(index + 1);
        }
        public static string AfterFirst([NotNull] this string str, char spliter)
        {
            if (str == null) throw new ArgumentNullException(nameof(str));

            var index = str.IndexOf(spliter);
            return index < 1 ? str : str.Substring(index + 1);
        }
        public static string AfterFirst([NotNull] this string str, params char[] spliter)
        {
            if (str == null) throw new ArgumentNullException(nameof(str));

            var index = str.LastIndexOfAny(spliter);
            return index < 1 ? str : str.Substring(index + 1);
        }

        public static string AfterLast([NotNull] this string str, [NotNull] string spliter,
            StringComparison comparisonType = StringComparison.Ordinal)
        {
            if (str == null) throw new ArgumentNullException(nameof(str));
            if (string.IsNullOrEmpty(spliter)) throw new ArgumentException(nameof(spliter));

            var index = str.LastIndexOf(spliter, comparisonType);
            return index < 1 ? str : str.Substring(index + 1);
        }
        public static string AfterLast([NotNull] this string str, char spliter)
        {
            if (str == null) throw new ArgumentNullException(nameof(str));

            var index = str.LastIndexOf(spliter);
            return index < 1 ? str : str.Substring(index + 1);
        }
        public static string AfterLast([NotNull] this string str, params char[] spliter)
        {
            if (str == null) throw new ArgumentNullException(nameof(str));

            var index = str.LastIndexOfAny(spliter);
            return index < 1 ? str : str.Substring(index + 1);
        }

        public static string BeforeFirst([NotNull] this string str, [NotNull] string spliter,
            StringComparison comparisonType = StringComparison.Ordinal)
        {
            if (str == null) throw new ArgumentNullException(nameof(str));
            if (string.IsNullOrEmpty(spliter)) throw new ArgumentException(nameof(spliter));

            var index = str.IndexOf(spliter, comparisonType);
            if (index < 0) return str;
            return index == 0 ? string.Empty : str.Substring(0, index);
        }
        public static string BeforeFirst([NotNull] this string str, [NotNull] string[] spliters,
            StringComparison comparisonType = StringComparison.Ordinal)
        {
            if (str == null) throw new ArgumentNullException(nameof(str));
            if (spliters == null) throw new ArgumentNullException(nameof(spliters));
            if (spliters.Length == 0 || spliters.Any(string.IsNullOrEmpty)) throw new ArgumentException(nameof(spliters));

            var indexs = spliters.Select(z => str.IndexOf(z, comparisonType)).Where(z => z >= 0).ToArray();
            if (indexs.Length == 0) return str;
            var index = indexs.Min();
            return index == 0 ? string.Empty : str.Substring(0, index);
        }

        public static string BeforeLast([NotNull] this string str, [NotNull] string spliter,
            StringComparison comparisonType = StringComparison.Ordinal)
        {
            if (str == null) throw new ArgumentNullException(nameof(str));
            if (spliter == null) throw new ArgumentNullException(nameof(spliter));

            var index = str.LastIndexOf(spliter, comparisonType);
            return index <= 0 ? string.Empty : str.Substring(0, index);
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
