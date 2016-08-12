using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jasily.Text
{
    public class NoneEncoding : Encoding
    {
        #region Overrides of Encoding

        public override int GetByteCount(char[] chars, int index, int count)
        {
            chars.CheckRange(index, count);
            if (count == 0) return 0;
            return System.Convert.ToUInt16(chars[index + count - 1]) > byte.MaxValue ? count * 2 : count * 2 - 1;
        }

        public override int GetBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex)
        {
            bytes.CheckRange(byteIndex);
            chars.CheckRange(charIndex, charCount);

            var count = this.GetByteCount(chars, charIndex, charCount);
            if (count > bytes.Length + byteIndex) throw new ArgumentException();

            foreach (var value in chars.Skip(charIndex).Take(charCount))
            {
                var buf = BitConverter.GetBytes(value);
                bytes[byteIndex] = buf[0];
                if (buf[1] > 0)
                {
                    bytes[byteIndex + 1] = buf[1];
                    byteIndex += 2;
                }
                else
                {
                    byteIndex++;
                }
            }
            return count;
        }

        public override int GetCharCount(byte[] bytes, int index, int count)
        {
            bytes.CheckRange(index, count);
            return count / 2 + (count % 2 == 1 ? 1 : 0);
        }

        public override int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex)
        {
            chars.CheckRange(charIndex);
            var count = this.GetCharCount(bytes, byteIndex, byteCount);
            if (count == 0) return 0;
            if (count + charIndex > chars.Length) throw new ArgumentException();
            for (var i = 0; i + 1 < bytes.Length; i += 2)
            {
                chars[charIndex] = BitConverter.ToChar(bytes, i);
                charIndex++;
            }
            if (bytes.Length % 2 == 1)
            {
                chars[charIndex] = (char)bytes[bytes.Length - 1];
            }
            return count;
        }

        public override int GetMaxByteCount(int charCount) => charCount / 2 + 1;

        public override int GetMaxCharCount(int byteCount) => byteCount / 2 + 1;

        #endregion
    }
}