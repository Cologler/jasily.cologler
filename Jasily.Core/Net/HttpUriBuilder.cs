using System.Collections.Generic;
using System.Linq;

namespace System.Net
{
    public class HttpUriBuilder : IBuilder<Uri>
    {
        private readonly string _uriString;
        private readonly List<KeyValuePair<string, string>> _queryStringParameter;

        public HttpUriBuilder(string uriString)
        {
            _queryStringParameter = new List<KeyValuePair<string, string>>();
            _uriString = uriString;
        }

        public void AddQueryStringParameter(string key, string value)
        {
            _queryStringParameter.Add(new KeyValuePair<string, string>(key, value));
        }

        public Uri Build()
        {
            var parameter = _queryStringParameter.Count == 0
                ? ""
                : "?" + String.Join("&", _queryStringParameter.Select(z => WebUtility.UrlEncode(z.Key) + "=" + WebUtility.UrlEncode(z.Value)));

            return new Uri(_uriString + parameter, UriKind.Absolute);
        }
    }
}