using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
