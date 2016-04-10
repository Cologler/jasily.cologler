using JetBrains.Annotations;
using System.Diagnostics;

namespace System.Text
{
    public static class JasilyEncoding
    {
        private static Encoding gb2312;

        public static Encoding GB2312 => gb2312;

        public static Encoding GetEncoding(string name)
        {
            try
            {
                return Encoding.GetEncoding(name);
            }
            catch
            {
                // ignored
            }

            Encoding encoding = null;
            switch (name)
            {
                case "gbk":
                case "gb2312":
                    encoding = GB2312;
                    break;
            }
            if (encoding == null) throw new NotSupportedException();
            return encoding;
        }

        public static void Register([NotNull] this Encoding encoding)
        {
            if (encoding == null) throw new ArgumentNullException(nameof(encoding));

            switch (encoding.WebName)
            {
                case "gb2312":
                    gb2312 = encoding;
                    break;

                default:
                    if (Debugger.IsAttached) Debugger.Break();
                    break;
            }
        }
    }
}