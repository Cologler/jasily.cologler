using System.Text.RegularExpressions;
using JetBrains.Annotations;

namespace System
{
    public static class Can
    {
        public static bool CreateUri([NotNull] string uriString, UriKind uriKind)
        {
            if (uriString == null) throw new ArgumentNullException(nameof(uriString));
            Uri uri;
            return Uri.TryCreate(uriString, uriKind, out uri);
        }

        public static bool CreateRegex([NotNull] string pattern)
        {
            try
            {
                // ReSharper disable once ObjectCreationAsStatement
                new Regex(pattern);
                return true;
            }
            catch (ArgumentException)
            {
                return false;
            }
        }

        public static bool CreateRegex(string pattern, RegexOptions options)
        {
            try
            {
                // ReSharper disable once ObjectCreationAsStatement
                new Regex(pattern, options);
                return true;
            }
            catch (ArgumentException)
            {
                return false;
            }
        }
    }
}