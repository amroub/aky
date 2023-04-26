namespace aky.Foundation.Utility.HttpClient
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using aky.Foundation.Utility.HttpClient.Content;
    using aky.Foundation.Utility.Serialization;
    using Polly;

    public class ResilientHttpClient : IHttpClient
    {
        private ISerializer serializer;

        public string AcceptHeader { get; } = "application/json";

        public TimeSpan Timeout { get; set; } = new TimeSpan(0, 0, 15);

        private Policy RetryPolicy { get; set; }

        public ResilientHttpClient(ISerializer serializer, Policy policy)
        {
            // Add Policies to be applied
            this.RetryPolicy = policy;
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

            return await this.HttpInvoker(async () =>
             {
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
                 System.Net.Http.HttpClient client = new System.Net.Http.HttpClient();

                 var response = await client.SendAsync(request);

                 // raise exception if HttpResponseCode 500
                 if (response.StatusCode == HttpStatusCode.InternalServerError)
                 {
                     throw new HttpRequestException();
                 }

                 return response;
             });
        }

        private async Task<T> HttpInvoker<T>(Func<Task<T>> action)
        {
            // Executes the action applying all
            // the policies defined in the wrapper
            return await this.RetryPolicy.ExecuteAsync(() => action());
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
