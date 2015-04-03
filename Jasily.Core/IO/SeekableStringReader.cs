using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            ReadedBuffer = new StringBuilder();
            _stringLength = s.Length;
        }

        public void Seek(int offset, SeekOrigin origin)
        {
            switch (origin)
            {
                case SeekOrigin.Begin:
                    while (offset < 0)
                        offset += _stringLength;

                    while (offset > _stringLength)
                        offset -= _stringLength;

                    while (offset > ReadedBuffer.Length)
                    {
                        Read();
                    }

                    _position = offset;
                    break;

                case SeekOrigin.Current:
                    this.Seek(offset + _position, SeekOrigin.Begin);
                    break;

                case SeekOrigin.End:
                    this.Seek(-offset, SeekOrigin.Begin);
                    break;
            }
        }

        public override int Read()
        {
            if (ReadedBuffer.Length > _position)
            {
                return (int)ReadedBuffer[_position++];
            }
            else
            {
                return Append(base.Read());
            }
        }

        public override int Read(char[] buffer, int index, int count)
        {
            var readed = 0;
            if (ReadedBuffer.Length > _position)
            {
                readed = Math.Min(ReadedBuffer.Length - _position, count);
                this.ReadedBuffer.CopyTo(_position, buffer, index, readed);
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
            return Append(base.ReadToEnd());
        }

        private int Append(int ch)
        {
            if (ch != -1)
            {
                ReadedBuffer.Append((char)ch);
                _position++;
            }
            return ch;
        }

        private string Append(string str)
        {
            if (str != null)
            {
                ReadedBuffer.Append(str);
                _position += str.Length;
            }
            return str;
        }

        private void Append(char[] value, int startIndex, int charCount)
        {
            ReadedBuffer.Append(value, startIndex, charCount);
            _position += charCount;
        }
    }
}
