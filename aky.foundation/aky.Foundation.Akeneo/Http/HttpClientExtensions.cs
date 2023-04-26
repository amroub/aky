using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Akeneo.Serialization;
using Newtonsoft.Json;

namespace Akeneo.Http
{
    public static class HttpClientExtensions
    {
        public static Task<HttpResponseMessage> PostJsonAsync<TContent>(this HttpClient client, string uri, TContent content, JsonSerializerSettings setting)
        {
            return client.PostAsync(uri, new JsonContent(content, setting));
        }

        public static Task<HttpResponseMessage> PostJsonAsync<TContent>(this HttpClient client, string uri, TContent content)
        {
            return client.PostJsonAsync(uri, content, AkeneoSerializerSettings.Create);
        }

        public static Task<HttpResponseMessage> PatchAsJsonAsync<TContent>(this HttpClient client, string requestUri, TContent content)
        {
            return client.PatchAsJsonAsync(requestUri, content, AkeneoSerializerSettings.Update);
        }

        public static Task<HttpResponseMessage> PatchAsJsonAsync<TContent>(this HttpClient client, string requestUri, TContent content, JsonSerializerSettings setting)
        {
            return client.PatchAsync(requestUri, new JsonContent(content, setting));
        }

        public static Task<HttpResponseMessage> PatchAsJsonCollectionAsync<TContent>(this HttpClient client, string requestUri, IEnumerable<TContent> content)
        {
            return client.PatchAsync(requestUri, new JsonCollectionContent<TContent>(content));
        }

        public static Task<HttpResponseMessage> PatchAsync(this HttpClient client, string requestUri, HttpContent content)
        {
            var request = new HttpRequestMessage(
                new HttpMethod("PATCH"),
                new Uri(requestUri, UriKind.Relative))
            {
                Content = content
            };
            return client.SendAsync(request);
        }
    }
}
