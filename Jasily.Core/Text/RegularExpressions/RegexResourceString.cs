using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Text.RegularExpressions
{
    public static class RegexResourceString
    {
        /// <summary>
        /// match url begin with any scheme
        /// <para/>[a-zA-z]+://\S*
        /// </summary>
        public const string Url = @"[a-zA-z]+://\S*";

        /// <summary>
        /// match url begin with http:// or https://
        /// <para/>http[s]?://\S*
        /// </summary>
        public const string HttpUrl = @"http[s]?://\S*";
    }
}
