using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Jasily.Net
{
    public class HttpUriBuilder : IBuilder<string>, IBuilder<Uri>
    {
        private readonly string baseUri;

        /// <summary>
        /// some url need same key, so can not use dict.
        /// </summary>
        private readonly List<KeyValuePair<string, string>> parameters
            = new List<KeyValuePair<string, string>>();

        public HttpUriBuilder(string uriString)
        {
            if (uriString == null) throw new ArgumentNullException(nameof(uriString));

            int index;
            if ((index = uriString.IndexOf("?", StringComparison.Ordinal)) != -1)
            {
                this.baseUri = uriString.Substring(0, index);
                var parameters = uriString.Substring(index + 1);
                if (parameters.Length > 0)
                {
                    foreach (var pair in parameters.Split('&'))
                    {
                        var kvp = pair.Split('=');
                        if (kvp.Length != 2) throw new FormatException();
                        this.AddParameter(kvp[0], kvp[1]);
                    }
                }
            }
            else
                this.baseUri = uriString;
        }

        public void AddParameter([NotNull] string key, string value)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));

            this.parameters.Add(new KeyValuePair<string, string>(key, value));
        }

        public void AddParameterIfNotNull(string key, string value)
        {
            if (value != null) this.AddParameter(key, value);
        }

        public void AddParameterIfNotEmpty(string key, string value)
        {
            if (!value.IsNullOrEmpty()) this.AddParameter(key, value);
        }

        public void AddParameterIfNotWhiteSpace(string key, string value)
        {
            if (!value.IsNullOrWhiteSpace()) this.AddParameter(key, value);
        }

        public void RemoveFirstParameter([NotNull] Predicate<KeyValuePair<string, string>> predicate)
        {
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            for (var i = 0; i < this.parameters.Count; i++)
            {
                if (predicate(this.parameters[i]))
                {
                    this.parameters.RemoveAt(i);
                    return;
                }
            }
        }

        public void RemoveAllParameters(Predicate<KeyValuePair<string, string>> predicate = null)
        {
            if (predicate == null)
            {
                this.parameters.Clear();
            }
            else
            {
                this.parameters.RemoveAll(predicate);
            }
        }

        public bool IsEncodeParameterKey { get; set; } = true;

        public bool IsEncodeParameterValue { get; set; } = true;

        public string Build()
        {
            var uri = this.baseUri;
            if (this.parameters.Count != 0)
            {
                uri = uri + "?" + string.Join("&", this.parameters.Select(this.EncodingParameter));
            }
            return uri;
        }

        Uri IBuilder<Uri>.Build() => new Uri(this.Build(), UriKind.Absolute);

        private string EncodingParameter(KeyValuePair<string, string> kvp)
        {
            return kvp.Value == null
                ? this.EncodingParameterKey(kvp.Key) + "="
                : this.EncodingParameterKey(kvp.Key) + "=" + this.EncodingParameterValue(kvp.Value);
        }

        private string EncodingParameterKey(string key)
            => this.IsEncodeParameterKey ? WebUtility.UrlEncode(key) : key;

        private string EncodingParameterValue(string value)
            => this.IsEncodeParameterValue ? WebUtility.UrlEncode(value) : value;
    }
}