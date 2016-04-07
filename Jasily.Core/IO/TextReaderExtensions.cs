using JetBrains.Annotations;
using System.Collections.Generic;
using System.Linq;

namespace System.IO
{
    public static class TextReaderExtensions
    {
        /// <summary>
        /// read next char if not end.
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static char? ReadChar([NotNull] this TextReader reader)
        {
            if (reader == null) throw new ArgumentNullException(nameof(reader));
            var n = reader.Read();
            return n == -1 ? (char?)null : (char)n;
        }

        [NotNull]
        [ItemNotNull]
        public static IEnumerable<string> EnumerateLines([NotNull] this TextReader reader)
        {
            if (reader == null) throw new ArgumentNullException(nameof(reader));
            while (true)
            {
                var line = reader.ReadLine();
                if (line == null) yield break;
                yield return line;
            }
        }

        [NotNull]
        [ItemNotNull]
        public static IEnumerable<char> EnumerateChars([NotNull] this TextReader reader)
        {
            if (reader == null) throw new ArgumentNullException(nameof(reader));
            while (true)
            {
                var ch = reader.Read();
                if (ch == -1) yield break;
                yield return (char)ch;
            }
        }

        [NotNull]
        [ItemNotNull]
        public static IEnumerable<char[]> EnumerateBlocks([NotNull] this TextReader reader, int maxBlockSize)
        {
            if (reader == null) throw new ArgumentNullException(nameof(reader));
            while (true)
            {
                var block = new char[maxBlockSize];
                var n = reader.ReadBlock(block, 0, maxBlockSize);
                if (n == maxBlockSize)
                {
                    yield return block;
                }
                else
                {
                    if (n == 0)
                    {
                        yield break;
                    }
                    else
                    {
                        yield return block.Take(n).ToArray();
                        yield break;
                    }
                }
            }
        }
    }
}