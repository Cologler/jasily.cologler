using System.Collections.Generic;
using System.Linq;

namespace System.Net
{
    public class HttpUriBuilder : IBuilder<Uri>
    {
        private readonly string _uriString;

        public List<KeyValuePair<string, string>> QueryStringParameters { get; private set; }

        public HttpUriBuilder(string uriString)
        {
            QueryStringParameters = new List<KeyValuePair<string, string>>();
            _uriString = uriString;
        }

        public void AddQueryStringParameter(string key, string value)
        {
            QueryStringParameters.Add(new KeyValuePair<string, string>(key, value));
        }

        public Uri Build()
        {
            var parameter = QueryStringParameters.Count == 0
                ? ""
                : "?" + String.Join("&", QueryStringParameters.Select(z => WebUtility.UrlEncode(z.Key) + "=" + WebUtility.UrlEncode(z.Value)));

            return new Uri(_uriString + parameter, UriKind.Absolute);
        }
    }
}