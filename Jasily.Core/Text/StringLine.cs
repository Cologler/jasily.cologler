using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace System.Text
{
    public class StringLine : IEnumerable<string>
    {
        public const string WindowsLineSplit = "\r\n";
        public const string UnixLineSplit = "\n";

        private List<string> InnerLines;

        public StringLine()
        {
            this.InnerLines = new List<string>();
        }
        public StringLine(string line)
            : this()
        {
            this.Append(line);
        }
        public StringLine(IEnumerable<string> lines)
            : this()
        {
            foreach (var line in lines)
                this.Append(line);
        }

        public void Append(string line)
        {
            if (line == null) throw new ArgumentNullException("line");

            InnerLines.AddRange(line.Split(new string[] {WindowsLineSplit, UnixLineSplit}, StringSplitOptions.None));
        }

        public IEnumerator<string> GetEnumerator()
        {
            return InnerLines.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public StringLine PadLeft(int totalWidth)
        {
            return new StringLine(InnerLines.Select(z => z.PadLeft(totalWidth)));
        }

        public StringLine PadLeft(int totalWidth, char paddingChar)
        {
            return new StringLine(InnerLines.Select(z => z.PadLeft(totalWidth, paddingChar)));
        }

        public StringLine PadRight(int totalWidth)
        {
            return new StringLine(InnerLines.Select(z => z.PadRight(totalWidth)));
        }

        public StringLine PadRight(int totalWidth, char paddingChar)
        {
            return new StringLine(InnerLines.Select(z => z.PadRight(totalWidth, paddingChar)));
        }
    }
}