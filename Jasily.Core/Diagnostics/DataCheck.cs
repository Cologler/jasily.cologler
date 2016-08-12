using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace System.Diagnostics
{
    public static class DataCheck
    {
        internal static string Format(string path, int line)
            => $"[{path}] ({line})";

        public static void NotNull<T>([NotNull] T value,
            [CallerFilePath] string path = null, [CallerLineNumber] int line = 0)
        {
            if (value == null)
            {
                throw new InvalidDataException($"{Format(path, line)} value cannot be null.");
            }
        }

        public static void NotEmpty<T>([NotNull] ICollection<T> collection,
            [CallerFilePath] string path = null, [CallerLineNumber] int line = 0)
        {
            if (collection.Count == 0)
            {
                throw new InvalidDataException($"{Format(path, line)} item of sequence connot be zero.");
            }
        }

        public static void NotEmpty<T>([NotNull] IEnumerable<T> numerable,
            [CallerFilePath] string path = null, [CallerLineNumber] int line = 0)
        {
            if (!numerable.Any())
            {
                throw new InvalidDataException($"{Format(path, line)} item of sequence connot be zero.");
            }
        }

        public static void ItemNotEmpty([NotNull] IEnumerable<string> numerable,
            [CallerFilePath] string path = null, [CallerLineNumber] int line = 0)
        {
            if (numerable.Any(string.IsNullOrEmpty))
            {
                throw new InvalidDataException($"{Format(path, line)} item of string sequence connot be null or empty.");
            }
        }

        public static void ItemNotWhiteSpace([NotNull] IEnumerable<string> numerable,
            [CallerFilePath] string path = null, [CallerLineNumber] int line = 0)
        {
            if (numerable.Any(string.IsNullOrWhiteSpace))
            {
                throw new InvalidDataException($"{Format(path, line)} item of string sequence connot be null or whiteSpace.");
            }
        }

        /// <summary>
        /// not NullOrEmpty
        /// </summary>
        /// <param name="value"></param>
        /// <param name="path"></param>
        /// <param name="line"></param>
        public static void NotEmpty([NotNull] string value,
            [CallerFilePath] string path = null, [CallerLineNumber] int line = 0)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new InvalidDataException($"{Format(path, line)} string value cannot be null or empty.");
            }
        }

        /// <summary>
        /// not NullOrWhiteSpace
        /// </summary>
        /// <param name="value"></param>
        /// <param name="path"></param>
        /// <param name="line"></param>
        public static void NotWhiteSpace([NotNull] string value,
            [CallerFilePath] string path = null, [CallerLineNumber] int line = 0)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new InvalidDataException($"{Format(path, line)} string value cannot be null or whiteSpace.");
            }
        }

        /// <summary>
        /// value must be true.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="path"></param>
        /// <param name="line"></param>
        public static void True(bool value,
            [CallerFilePath] string path = null, [CallerLineNumber] int line = 0)
        {
            if (!value)
            {
                throw new InvalidDataException($"{Format(path, line)} bool value cannot be false.");
            }
        }

        /// <summary>
        /// value must be false.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="path"></param>
        /// <param name="line"></param>
        public static void False(bool value,
            [CallerFilePath] string path = null, [CallerLineNumber] int line = 0)
        {
            if (value)
            {
                throw new InvalidDataException($"{Format(path, line)} bool value cannot be true.");
            }
        }
    }
}