namespace aky.Foundation.Utility.HttpClient
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using aky.Foundation.Utility.HttpClient.Content;
    using aky.Foundation.Utility.Serialization;

    public class StandardHttpClient : IHttpClient
    {
        private ISerializer serializer;

        public string AcceptHeader { get; } = "application/json";

        public TimeSpan Timeout { get; set; } = new TimeSpan(0, 0, 15);

        public StandardHttpClient(ISerializer serializer)
        {
            this.serializer = serializer;
        }

        public async Task<HttpResponseMessage> GetAsync(string uri, string authorizationToken = null, string authorizationMethod = "Bearer", Dictionary<string, string> customHeaders = null)
        {
            return await this.SendAsync(uri, HttpMethod.Get, null, authorizationToken, authorizationMethod, customHeaders);
        }

        public async Task<HttpResponseMessage> PostAsync(string uri, object value, string authorizationToken = null, string authorizationMethod = "Bearer", Dictionary<string, string> customHeaders = null)
        {
            var content = new JsonContent(this.serializer, value);

            return await this.SendAsync(uri, HttpMethod.Post, content, authorizationToken, authorizationMethod, customHeaders);
        }

        public async Task<HttpResponseMessage> PutAsync(string uri, object value, string authorizationToken = null, string authorizationMethod = "Bearer", Dictionary<string, string> customHeaders = null)
        {
            var content = new JsonContent(this.serializer, value);

            return await this.SendAsync(uri, HttpMethod.Put, content, authorizationToken, authorizationMethod, customHeaders);
        }

        public async Task<HttpResponseMessage> PatchAsync(string uri, object value, string authorizationToken = null, string authorizationMethod = "Bearer", Dictionary<string, string> customHeaders = null)
        {
            var content = new JsonContent(this.serializer, value);

            return await this.SendAsync(uri, new HttpMethod("PATCH"), content, authorizationToken, authorizationMethod, customHeaders);
        }

        public async Task<HttpResponseMessage> DeleteAsync(string uri, string authorizationToken = null, string authorizationMethod = "Bearer", Dictionary<string, string> customHeaders = null)
        {
            return await this.SendAsync(uri, HttpMethod.Delete, null, authorizationToken, authorizationMethod, customHeaders);
        }

        private async Task<HttpResponseMessage> SendAsync(
            string uri,
            HttpMethod method,
            HttpContent content = null,
            string authorizationToken = null,
            string authorizationMethod = "Bearer",
            Dictionary<string, string> customHeaders = null)
        {
            // Check required arguments
            this.EnsureArguments(uri, method);

            // Setup request
            var request = new HttpRequestMessage
            {
                Method = method,
                RequestUri = new Uri(uri),
            };

            if (content != null)
            {
                request.Content = content;
            }

            if (!string.IsNullOrEmpty(authorizationToken))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue(authorizationMethod, authorizationToken);
            }

            request.Headers.Accept.Clear();
            if (!string.IsNullOrEmpty(this.AcceptHeader))
            {
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(this.AcceptHeader));
            }

            if (customHeaders != null)
            {
                foreach (var header in customHeaders)
                {
                    request.Headers.Add(header.Key, header.Value);
                }
            }

            // Setup client
            var client = new System.Net.Http.HttpClient();
            client.Timeout = this.Timeout;

            return await client.SendAsync(request);
        }

        private void EnsureArguments(string requestUri, HttpMethod method)
        {
            if (method == null)
            {
                throw new ArgumentNullException(nameof(method));
            }

            if (string.IsNullOrEmpty(requestUri))
            {
                throw new ArgumentNullException(nameof(requestUri));
            }
        }
    }
}
