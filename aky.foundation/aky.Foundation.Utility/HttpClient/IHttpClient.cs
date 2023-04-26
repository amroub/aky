namespace aky.Foundation.Utility.HttpClient
{
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;

    public interface IHttpClient
    {
        Task<HttpResponseMessage> GetAsync(string uri, string authorizationToken = null, string authorizationMethod = "Bearer", Dictionary<string, string> customHeaders = null);

        Task<HttpResponseMessage> PostAsync(string uri, object value, string authorizationToken = null, string authorizationMethod = "Bearer", Dictionary<string, string> customHeaders = null);

        Task<HttpResponseMessage> PutAsync(string uri, object value, string authorizationToken = null, string authorizationMethod = "Bearer", Dictionary<string, string> customHeaders = null);

        Task<HttpResponseMessage> PatchAsync(string uri, object value, string authorizationToken = null, string authorizationMethod = "Bearer", Dictionary<string, string> customHeaders = null);

        Task<HttpResponseMessage> DeleteAsync(string uri, string authorizationToken = null, string authorizationMethod = "Bearer", Dictionary<string, string> customHeaders = null);
    }
}
