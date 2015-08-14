
namespace System.Net
{
    public static class HttpWebRequestResourceString
    {
        public static class Method
        {
            public const string Get = "GET";
            public const string Post = "POST";
            public const string Put = "PUT";
            public const string Patch = "PATCH";
            public const string Delete = "DELETE";
        }

        public static class ContentType
        {
            public static class Application
            {
                /// <summary>
                /// return "application/json"
                /// </summary>
                public const string Json = "application/json";

                /// <summary>
                /// return "application/x-www-form-urlencoded"
                /// </summary>
                public const string XWwwFormUrlencoded = "application/x-www-form-urlencoded";
            }

            public static class CharSet
            {
                /// <summary>
                /// return "charset=utf-8"
                /// </summary>
                public const string Utf8 = "charset=utf-8";
            }
        }
    }
}
