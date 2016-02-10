using System;

namespace Jasily.Net.Http
{
    public struct RangeHeader : IBuilder<string>
    {
        public long From { get; private set; }

        public long? To { get; private set; }

        public RangeHeader(long from, long to)
            : this(from)
        {
            this.To = to;
        }

        public RangeHeader(long from)
            : this()
        {
            this.From = from;
        }

        public override string ToString() => this.Build();

        public static RangeHeader? TryParse(string rangeHeader)
        {
            if (!rangeHeader.StartsWith("bytes="))
                return null;

            var range = rangeHeader.Substring(6);
            if (range.Length == 0)
                return null;

            var array = range.Split(new[] { '-' }, 2);
            if (array.Length != 2)
                return null;

            int from;
            if (array[1].IsNullOrWhiteSpace())
            {
                if (int.TryParse(array[0], out from))
                    return new RangeHeader(from);
            }
            else
            {
                int to;
                if (int.TryParse(array[0], out from) && int.TryParse(array[0], out to))
                    return new RangeHeader(from, to);
            }

            return null;
        }

        public string Build() => $"bytes={this.From}-{this.To}";
    }
}