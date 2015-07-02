using System.Collections.Generic;
using System.Linq;

namespace System.Net
{
    public class HttpUriBuilder : IBuilder<Uri>
    {
        private readonly string _uriString;

        /// <summary>
        /// some url need same key, so can not use dict !!!
        /// </summary>
        public List<KeyValuePair<string, string>> QueryStringParameters { get; private set; }

        public HttpUriBuilder(string uriString)
        {
            this.QueryStringParameters = new List<KeyValuePair<string, string>>();
            this._uriString = uriString;
            this.IsEncodeParameterKey = true;
            this.IsEncodeParameterValue = true;
        }

        public void AddQueryStringParameter(string key, string value)
        {
            this.QueryStringParameters.Add(
                new KeyValuePair<string, string>(
                    key.ThrowIfNullOrEmpty("key"),
                    value.ThrowIfNull("value")));
        }

        public bool IsEncodeParameterKey { get; set; }

        public bool IsEncodeParameterValue { get; set; }

        public Uri Build()
        {
            return new Uri(this._uriString + this.BuildParameter(), UriKind.Absolute);
        }

        private string BuildParameter()
        {
            return this.QueryStringParameters.Count == 0
                ? ""
                : "?" + String.Join("&", this.QueryStringParameters.Select(this.EncodingParameter));
        }

        private string EncodingParameter(KeyValuePair<string, string> kvp)
        {
            return this.EncodingParameterKey(kvp.Key) + "=" + this.EncodingParameterValue(kvp.Value);
        }

        private string EncodingParameterKey(string key)
        {
            return this.IsEncodeParameterKey ? WebUtility.UrlEncode(key) : key;
        }

        private string EncodingParameterValue(string value)
        {
            return this.IsEncodeParameterValue ? WebUtility.UrlEncode(value) : value;
        }
    }
}