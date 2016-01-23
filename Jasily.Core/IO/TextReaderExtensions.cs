using System.Collections.Generic;
using System.Linq;

namespace System.IO
{
    public static class TextReaderExtensions
    {
        public static IEnumerable<string> EnumerateLines(this TextReader reader)
        {
            while (true)
            {
                var line = reader.ReadLine();
                if (line == null) yield break;
                yield return line;
            }
        }

        public static IEnumerable<char> EnumerateChars(this TextReader reader)
        {
            while (true)
            {
                var ch = reader.Read();
                if (ch == -1) yield break;
                yield return (char)ch;
            }
        }

        public static IEnumerable<char[]> EnumerateBlocks(this TextReader reader, int maxBlockSize)
        {
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