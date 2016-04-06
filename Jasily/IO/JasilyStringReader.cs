using JetBrains.Annotations;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Jasily.IO
{
    public class JasilyStringReader : TextReader
    {
        private readonly string s;
        private int pos; // next pos
        private readonly int length;

        public JasilyStringReader(string s)
        {
            if (s == null) throw new ArgumentNullException(nameof(s));
            this.s = s;
            this.length = s.Length;
        }

        public void Seek(int offset, SeekOrigin origin)
        {
            switch (origin)
            {
                case SeekOrigin.Begin:
                    this.pos = offset;
                    break;

                case SeekOrigin.Current:
                    this.pos += offset;
                    break;

                case SeekOrigin.End:
                    this.pos = this.length;
                    this.pos += offset;
                    break;
            }

            if (this.pos > this.length) this.pos = this.length;
            if (0 > this.pos) this.pos = 0;
        }

        [Pure]
        public override int Peek()
        {
            if (this.pos == this.length) return -1;
            return this.s[this.pos];
        }

        public override int Read()
        {
            if (this.pos == this.length) return -1;
            return this.s[this.pos++];
        }

        public override int Read([NotNull] char[] buffer, int index, int count)
        {
            if (buffer == null) throw new ArgumentNullException(nameof(buffer));
            if (index < 0) throw new ArgumentOutOfRangeException(nameof(index), "must >= 0");
            if (count < 0) throw new ArgumentOutOfRangeException(nameof(count), "must >= 0");
            if (buffer.Length - index < count) throw new ArgumentException("buffer over flow");

            var n = this.length - this.pos;
            if (n > 0)
            {
                if (n > count) n = count;
                this.s.CopyTo(this.pos, buffer, index, n);
                this.pos += n;
            }
            return n;
        }

        public override string ReadToEnd()
        {
            var s = this.pos == 0 ? this.s : this.length == this.pos ? string.Empty : this.s.Substring(this.pos, this.length - this.pos);
            this.pos = this.length;
            return s;
        }

        public override string ReadLine()
        {
            var i = this.pos;
            while (i < this.length)
            {
                var ch = this.s[i];
                if (ch == '\r' || ch == '\n')
                {
                    var result = this.s.Substring(this.pos, i - this.pos);
                    this.pos = i + 1;
                    if (ch == '\r' && this.pos < this.length && this.s[this.pos] == '\n') this.pos++;
                    return result;
                }
                i++;
            }
            if (i > this.pos)
            {
                var result = this.s.Substring(this.pos, i - this.pos);
                this.pos = i;
                return result;
            }
            return null;
        }

        public override Task<string> ReadLineAsync() => Task.FromResult(this.ReadLine());

        public override Task<string> ReadToEndAsync() => Task.FromResult(this.ReadToEnd());

        public override Task<int> ReadBlockAsync([NotNull] char[] buffer, int index, int count) => Task.FromResult(this.ReadBlock(buffer, index, count));

        public override Task<int> ReadAsync([NotNull] char[] buffer, int index, int count) => Task.FromResult(this.Read(buffer, index, count));
    }
}