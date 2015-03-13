using System.Text;

namespace System
{
    public static class JasilyString
    {
        public static byte[] GetBytes(this string text, Encoding encoding)
        {
            return encoding.GetBytes(text);
        }
        public static byte[] GetBytes(this string text)
        {
            return text.GetBytes(Encoding.UTF8);
        }
    }
}
