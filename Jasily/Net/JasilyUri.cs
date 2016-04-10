using JetBrains.Annotations;
using System;
using System.Collections.Generic;

namespace Jasily.Net
{
    public class JasilyUri
    {
        public string Scheme { get; set; }

        public string Path { get; set; }

        public List<KeyValuePair<string, string>> Arguments { get; } = new List<KeyValuePair<string, string>>();

        public static JasilyUri Parse([NotNull] string url)
        {
            if (url == null) throw new ArgumentNullException(nameof(url));

            var uri = new JasilyUri();

            var schemePtr = url.IndexOf("://", StringComparison.Ordinal);
            uri.Scheme = schemePtr < 0 ? string.Empty : url.Take(schemePtr);

            var argsPtr = url.IndexOf("?", schemePtr, StringComparison.Ordinal);
            if (argsPtr >= 0)
            {
                var args = url.Substring(argsPtr + 1).Split("&", StringSplitOptions.RemoveEmptyEntries);
                foreach (var arg in args)
                {
                    var ap = arg.Split("=", 2);
                    uri.Arguments.Add(new KeyValuePair<string, string>(ap[0], ap.Length == 1 ? string.Empty : ap[1]));
                }
            }

            var path = url.Substring(schemePtr + 3, argsPtr);
            uri.Path = path;

            return uri;
        }
    }
}