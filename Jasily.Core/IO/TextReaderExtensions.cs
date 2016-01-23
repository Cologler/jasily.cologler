using System.Collections.Generic;

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
    }
}