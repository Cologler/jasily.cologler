
namespace System.Net
{
    public static class HttpWebRequestResourceString
    {
	    public static class Method
	    {
            public const string Get = "GET";
            public const string Post = "POST";
            public const string Put = "PUT";
        }

        public static class ContentType
        {
            /// <summary>
            /// return "application/json"
            /// </summary>
            public const string ApplicationJson = "application/json";

            /// <summary>
            /// return "application/x-www-form-urlencoded"
            /// </summary>
	        public const string ApplicationXWwwFormUrlencoded = "application/x-www-form-urlencoded";

            /// <summary>
            /// return "charset=utf-8"
            /// </summary>
            public const string CharSetUtf8 = "charset=utf-8";
        }
    }
}
