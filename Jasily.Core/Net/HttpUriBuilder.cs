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
            this.QueryStringParameters = new List<KeyValuePair<string, string>>();
            this._uriString = uriString;
        }

        public void AddQueryStringParameter(string key, string value)
        {
            this.QueryStringParameters.Add(
                new KeyValuePair<string, string>(
                    key.ThrowIfNullOrEmpty("key"),
                    value.ThrowIfNull("value")));
        }

        public Uri Build()
        {
            var parameter = this.QueryStringParameters.Count == 0
                ? ""
                : "?" + String.Join("&", this.QueryStringParameters.Select(z => WebUtility.UrlEncode(z.Key) + "=" + WebUtility.UrlEncode(z.Value)));

            return new Uri(this._uriString + parameter, UriKind.Absolute);
        }
    }
}