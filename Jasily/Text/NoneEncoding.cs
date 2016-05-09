using System;
using System.Text;

namespace Jasily.Text
{
    public class NoneEncoding : Encoding
    {
        #region Overrides of Encoding

        /// <summary>������������дʱ�������ָ���ַ������е�һ���ַ����б������������ֽ�����</summary>
        /// <returns>��ָ���ַ����б�������ɵ��ֽ�����</returns>
        /// <param name="chars">����Ҫ������ַ������ַ����顣</param>
        /// <param name="index">��һ��Ҫ������ַ���������</param>
        /// <param name="count">Ҫ������ַ�����Ŀ��</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="chars" /> Ϊ null��</exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// <paramref name="index" /> �� <paramref name="count" /> С���㡣 - �� - <paramref name="index" /> �� <paramref name="count" /> ����ʾ <paramref name="chars" /> �е���Ч��Χ��</exception>
        /// <exception cref="T:System.Text.EncoderFallbackException">�������ˣ���μ�.NET Framework �е��ַ������Ի�������Ľ��ͣ� ���ͣ� �� <see cref="P:System.Text.Encoding.EncoderFallback" /> ����Ϊ <see cref="T:System.Text.EncoderExceptionFallback" />��</exception>
        public override int GetByteCount(char[] chars, int index, int count)
        {
            chars.CheckArray(index, count);
            if (count == 0) return 0;
            return System.Convert.ToUInt16(chars[index + count - 1]) > byte.MaxValue ? count * 2 : count * 2 - 1;
        }

        /// <summary>������������дʱ����ָ���ַ������е�һ���ַ�����Ϊָ�����ֽ����顣</summary>
        /// <returns>д�� <paramref name="bytes" /> ��ʵ���ֽ�����</returns>
        /// <param name="chars">����Ҫ������ַ������ַ����顣</param>
        /// <param name="charIndex">��һ��Ҫ������ַ���������</param>
        /// <param name="charCount">Ҫ������ַ�����Ŀ��</param>
        /// <param name="bytes">Ҫ�������������ֽ����е��ֽ����顣</param>
        /// <param name="byteIndex">��ʼд�����������ֽ����е�����λ�á�</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="chars" /> Ϊ null�� - �� - <paramref name="bytes" /> Ϊ null��</exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// <paramref name="charIndex" />��<paramref name="charCount" /> �� <paramref name="byteIndex" /> С���㡣 - �� - <paramref name="charIndex" /> �� <paramref name="charCount" /> ����ʾ <paramref name="chars" /> �е���Ч��Χ�� - �� - <paramref name="byteIndex" /> ���� <paramref name="bytes" /> �е���Ч������</exception>
        /// <exception cref="T:System.ArgumentException">
        /// <paramref name="bytes" /> �д� <paramref name="byteIndex" /> �������βû���㹻���������������������ֽڡ�</exception>
        /// <exception cref="T:System.Text.EncoderFallbackException">�������ˣ���μ�.NET Framework �е��ַ������Ի�������Ľ��ͣ� ���ͣ� �� <see cref="P:System.Text.Encoding.EncoderFallback" /> ����Ϊ <see cref="T:System.Text.EncoderExceptionFallback" />��</exception>
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

        /// <summary>������������дʱ��������ֽ����У���ָ���ֽ����鿪ʼ�����н������������ַ�����</summary>
        /// <returns>��ָ���ֽ����н��н������������ַ�����</returns>
        /// <param name="bytes">����Ҫ������ֽ����е��ֽ����顣</param>
        /// <param name="index">��һ��Ҫ������ֽڵ�������</param>
        /// <param name="count">Ҫ������ֽ�����</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="bytes" /> Ϊ null��</exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// <paramref name="index" /> �� <paramref name="count" /> С���㡣 - �� - <paramref name="index" /> �� <paramref name="count" /> ����ʾ <paramref name="bytes" /> �е���Ч��Χ��</exception>
        /// <exception cref="T:System.Text.DecoderFallbackException">�������ˣ���μ�.NET Framework �е��ַ������Ի�������Ľ��ͣ� ���ͣ� �� <see cref="P:System.Text.Encoding.DecoderFallback" /> ����Ϊ <see cref="T:System.Text.DecoderExceptionFallback" />��</exception>
        public override int GetCharCount(byte[] bytes, int index, int count)
        {
            bytes.CheckArray(index, count);
            return count / 2 + (count % 2 == 1 ? 1 : 0);
        }

        /// <summary>������������дʱ����ָ���ֽ������е��ֽ����н���Ϊָ�����ַ����顣</summary>
        /// <returns>д�� <paramref name="chars" /> ��ʵ���ַ�����</returns>
        /// <param name="bytes">����Ҫ������ֽ����е��ֽ����顣</param>
        /// <param name="byteIndex">��һ��Ҫ������ֽڵ�������</param>
        /// <param name="byteCount">Ҫ������ֽ�����</param>
        /// <param name="chars">Ҫ���ڰ������������ַ������ַ����顣</param>
        /// <param name="charIndex">��ʼд�����������ַ���������λ�á�</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="bytes" /> Ϊ null�� - �� - <paramref name="chars" /> Ϊ null��</exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// <paramref name="byteIndex" />��<paramref name="byteCount" /> �� <paramref name="charIndex" /> С���㡣 - �� - <paramref name="byteindex" /> �� <paramref name="byteCount" /> ����ʾ <paramref name="bytes" /> �е���Ч��Χ�� - �� - <paramref name="charIndex" /> ���� <paramref name="chars" /> �е���Ч������</exception>
        /// <exception cref="T:System.ArgumentException">
        /// <paramref name="chars" /> �д� <paramref name="charIndex" /> �������βû���㹻�������������������ַ���</exception>
        /// <exception cref="T:System.Text.DecoderFallbackException">�������ˣ���μ�.NET Framework �е��ַ������Ի�������Ľ��ͣ� ���ͣ� �� <see cref="P:System.Text.Encoding.DecoderFallback" /> ����Ϊ <see cref="T:System.Text.DecoderExceptionFallback" />��</exception>
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

        /// <summary>������������дʱ�������ָ����Ŀ���ַ����б���������������ֽ�����</summary>
        /// <returns>��ָ����Ŀ���ַ����б���������������ֽ�����</returns>
        /// <param name="charCount">Ҫ������ַ�����Ŀ��</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// <paramref name="charCount" /> С���㡣</exception>
        /// <exception cref="T:System.Text.EncoderFallbackException">�������ˣ���μ�.NET Framework �е��ַ������Ի�������Ľ��ͣ� ���ͣ� �� <see cref="P:System.Text.Encoding.EncoderFallback" /> ����Ϊ <see cref="T:System.Text.EncoderExceptionFallback" />��</exception>
        public override int GetMaxByteCount(int charCount)
        {
            return charCount / 2 + 1;
        }

        /// <summary>������������дʱ�������ָ����Ŀ���ֽڽ��н���ʱ������������ַ�����</summary>
        /// <returns>��ָ����Ŀ���ֽڽ��н���ʱ������������ַ�����</returns>
        /// <param name="byteCount">Ҫ������ֽ�����</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// <paramref name="byteCount" /> С���㡣</exception>
        /// <exception cref="T:System.Text.DecoderFallbackException">�������ˣ���μ�.NET Framework �е��ַ������Ի�������Ľ��ͣ� ���ͣ� �� <see cref="P:System.Text.Encoding.DecoderFallback" /> ����Ϊ <see cref="T:System.Text.DecoderExceptionFallback" />��</exception>
        public override int GetMaxCharCount(int byteCount)
        {
            return byteCount / 2 + 1;
        }

        #endregion
    }
}