using System.Text;

namespace System.IO
{
    /// <summary>
    /// not tested.
    /// </summary>
    public class SeekableStringReader : StringReader
    {
        private StringBuilder ReadedBuffer;
        private int _stringLength;
        private int _position;

        public SeekableStringReader(string s)
            : base(s)
        {
            this.ReadedBuffer = new StringBuilder();
            this._stringLength = s.Length;
        }

        public void Seek(int offset, SeekOrigin origin)
        {
            switch (origin)
            {
                case SeekOrigin.Begin:
                    while (offset < 0)
                        offset += this._stringLength;

                    while (offset > this._stringLength)
                        offset -= this._stringLength;

                    while (offset > this.ReadedBuffer.Length)
                    {
                        this.Read();
                    }

                    this._position = offset;
                    break;

                case SeekOrigin.Current:
                    this.Seek(offset + this._position, SeekOrigin.Begin);
                    break;

                case SeekOrigin.End:
                    this.Seek(-offset, SeekOrigin.Begin);
                    break;
            }
        }

        public override int Read()
        {
            if (this.ReadedBuffer.Length > this._position)
            {
                return (int) this.ReadedBuffer[this._position++];
            }
            else
            {
                return this.Append(base.Read());
            }
        }

        public override int Read(char[] buffer, int index, int count)
        {
            var readed = 0;
            if (this.ReadedBuffer.Length > this._position)
            {
                readed = Math.Min(this.ReadedBuffer.Length - this._position, count);
                this.ReadedBuffer.CopyTo(this._position, buffer, index, readed);
            }
            if (readed < count)
            {
                var copyed = base.Read(buffer, index + readed, count - readed);
                this.Append(buffer, index + readed, copyed);
                return copyed + readed;
            }
            return readed;
        }

        /// <summary>
        /// alway throw NotSupportedException
        /// </summary>
        /// <exception cref="System.NotSupportedException"></exception>
        /// <returns></returns>
        public override string ReadLine()
        {
            throw new NotSupportedException();
        }

        public override string ReadToEnd()
        {
            return this.Append(base.ReadToEnd());
        }

        private int Append(int ch)
        {
            if (ch != -1)
            {
                this.ReadedBuffer.Append((char)ch);
                this._position++;
            }
            return ch;
        }

        private string Append(string str)
        {
            if (str != null)
            {
                this.ReadedBuffer.Append(str);
                this._position += str.Length;
            }
            return str;
        }

        private void Append(char[] value, int startIndex, int charCount)
        {
            this.ReadedBuffer.Append(value, startIndex, charCount);
            this._position += charCount;
        }
    }
}
