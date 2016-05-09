using System;
using System.Text;

namespace Jasily.Text
{
    public class NoneEncoding : Encoding
    {
        #region Overrides of Encoding

        /// <summary>在派生类中重写时，计算对指定字符数组中的一组字符进行编码所产生的字节数。</summary>
        /// <returns>对指定字符进行编码后生成的字节数。</returns>
        /// <param name="chars">包含要编码的字符集的字符数组。</param>
        /// <param name="index">第一个要编码的字符的索引。</param>
        /// <param name="count">要编码的字符的数目。</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="chars" /> 为 null。</exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// <paramref name="index" /> 或 <paramref name="count" /> 小于零。 - 或 - <paramref name="index" /> 和 <paramref name="count" /> 不表示 <paramref name="chars" /> 中的有效范围。</exception>
        /// <exception cref="T:System.Text.EncoderFallbackException">发生回退（请参见.NET Framework 中的字符编码以获得完整的解释） －和－ 将 <see cref="P:System.Text.Encoding.EncoderFallback" /> 设置为 <see cref="T:System.Text.EncoderExceptionFallback" />。</exception>
        public override int GetByteCount(char[] chars, int index, int count)
        {
            chars.CheckArray(index, count);
            if (count == 0) return 0;
            return System.Convert.ToUInt16(chars[index + count - 1]) > byte.MaxValue ? count * 2 : count * 2 - 1;
        }

        /// <summary>在派生类中重写时，将指定字符数组中的一组字符编码为指定的字节数组。</summary>
        /// <returns>写入 <paramref name="bytes" /> 的实际字节数。</returns>
        /// <param name="chars">包含要编码的字符集的字符数组。</param>
        /// <param name="charIndex">第一个要编码的字符的索引。</param>
        /// <param name="charCount">要编码的字符的数目。</param>
        /// <param name="bytes">要包含所产生的字节序列的字节数组。</param>
        /// <param name="byteIndex">开始写入所产生的字节序列的索引位置。</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="chars" /> 为 null。 - 或 - <paramref name="bytes" /> 为 null。</exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// <paramref name="charIndex" />、<paramref name="charCount" /> 或 <paramref name="byteIndex" /> 小于零。 - 或 - <paramref name="charIndex" /> 和 <paramref name="charCount" /> 不表示 <paramref name="chars" /> 中的有效范围。 - 或 - <paramref name="byteIndex" /> 不是 <paramref name="bytes" /> 中的有效索引。</exception>
        /// <exception cref="T:System.ArgumentException">
        /// <paramref name="bytes" /> 中从 <paramref name="byteIndex" /> 到数组结尾没有足够的容量来容纳所产生的字节。</exception>
        /// <exception cref="T:System.Text.EncoderFallbackException">发生回退（请参见.NET Framework 中的字符编码以获得完整的解释） －和－ 将 <see cref="P:System.Text.Encoding.EncoderFallback" /> 设置为 <see cref="T:System.Text.EncoderExceptionFallback" />。</exception>
        public override int GetBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex)
        {
            bytes.CheckArray(byteIndex);
            chars.CheckArray(charIndex, charCount);

            var count = this.GetByteCount(chars, charIndex, charCount);
            if (count > bytes.Length + byteIndex) throw new ArgumentException();

            foreach (var value in chars.Enumerate(charIndex, charCount))
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

        /// <summary>在派生类中重写时，计算对字节序列（从指定字节数组开始）进行解码所产生的字符数。</summary>
        /// <returns>对指定字节序列进行解码所产生的字符数。</returns>
        /// <param name="bytes">包含要解码的字节序列的字节数组。</param>
        /// <param name="index">第一个要解码的字节的索引。</param>
        /// <param name="count">要解码的字节数。</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="bytes" /> 为 null。</exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// <paramref name="index" /> 或 <paramref name="count" /> 小于零。 - 或 - <paramref name="index" /> 和 <paramref name="count" /> 不表示 <paramref name="bytes" /> 中的有效范围。</exception>
        /// <exception cref="T:System.Text.DecoderFallbackException">发生回退（请参见.NET Framework 中的字符编码以获得完整的解释） －和－ 将 <see cref="P:System.Text.Encoding.DecoderFallback" /> 设置为 <see cref="T:System.Text.DecoderExceptionFallback" />。</exception>
        public override int GetCharCount(byte[] bytes, int index, int count)
        {
            bytes.CheckArray(index, count);
            return count / 2 + (count % 2 == 1 ? 1 : 0);
        }

        /// <summary>在派生类中重写时，将指定字节数组中的字节序列解码为指定的字符数组。</summary>
        /// <returns>写入 <paramref name="chars" /> 的实际字符数。</returns>
        /// <param name="bytes">包含要解码的字节序列的字节数组。</param>
        /// <param name="byteIndex">第一个要解码的字节的索引。</param>
        /// <param name="byteCount">要解码的字节数。</param>
        /// <param name="chars">要用于包含所产生的字符集的字符数组。</param>
        /// <param name="charIndex">开始写入所产生的字符集的索引位置。</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="bytes" /> 为 null。 - 或 - <paramref name="chars" /> 为 null。</exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// <paramref name="byteIndex" />、<paramref name="byteCount" /> 或 <paramref name="charIndex" /> 小于零。 - 或 - <paramref name="byteindex" /> 和 <paramref name="byteCount" /> 不表示 <paramref name="bytes" /> 中的有效范围。 - 或 - <paramref name="charIndex" /> 不是 <paramref name="chars" /> 中的有效索引。</exception>
        /// <exception cref="T:System.ArgumentException">
        /// <paramref name="chars" /> 中从 <paramref name="charIndex" /> 到数组结尾没有足够容量来容纳所产生的字符。</exception>
        /// <exception cref="T:System.Text.DecoderFallbackException">发生回退（请参见.NET Framework 中的字符编码以获得完整的解释） －和－ 将 <see cref="P:System.Text.Encoding.DecoderFallback" /> 设置为 <see cref="T:System.Text.DecoderExceptionFallback" />。</exception>
        public override int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex)
        {
            chars.CheckArray(charIndex);
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

        /// <summary>在派生类中重写时，计算对指定数目的字符进行编码所产生的最大字节数。</summary>
        /// <returns>对指定数目的字符进行编码所产生的最大字节数。</returns>
        /// <param name="charCount">要编码的字符的数目。</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// <paramref name="charCount" /> 小于零。</exception>
        /// <exception cref="T:System.Text.EncoderFallbackException">发生回退（请参见.NET Framework 中的字符编码以获得完整的解释） －和－ 将 <see cref="P:System.Text.Encoding.EncoderFallback" /> 设置为 <see cref="T:System.Text.EncoderExceptionFallback" />。</exception>
        public override int GetMaxByteCount(int charCount)
        {
            return charCount / 2 + 1;
        }

        /// <summary>在派生类中重写时，计算对指定数目的字节进行解码时所产生的最大字符数。</summary>
        /// <returns>对指定数目的字节进行解码时所产生的最大字符数。</returns>
        /// <param name="byteCount">要解码的字节数。</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// <paramref name="byteCount" /> 小于零。</exception>
        /// <exception cref="T:System.Text.DecoderFallbackException">发生回退（请参见.NET Framework 中的字符编码以获得完整的解释） －和－ 将 <see cref="P:System.Text.Encoding.DecoderFallback" /> 设置为 <see cref="T:System.Text.DecoderExceptionFallback" />。</exception>
        public override int GetMaxCharCount(int byteCount)
        {
            return byteCount / 2 + 1;
        }

        #endregion
    }
}