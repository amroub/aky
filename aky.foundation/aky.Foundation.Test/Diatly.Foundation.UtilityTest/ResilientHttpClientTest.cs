namespace Diatly.Foundation.Test.Diatly.Foundation.UtilityTest
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using global::Diatly.Foundation.Test.Domain;
    using global::Diatly.Foundation.Utility.HttpClient;
    using Newtonsoft.Json;
    using Xunit;

    public class ResilientHttpClientTest : BaseFixture
    {
        private string defaultUrl = "http://localhost:5000";
        private IHttpClient resilientHttpClient;
        private int retryCount = 0;

        public ResilientHttpClientTest()
        {
            this.resilientHttpClient = this.ResilientHttpClient;
        }

        [Fact]
        public async Task ResilientHttpClient_Get_Pass()
        {
            string resourcePath = "/resilient-get";

            InMemoryWebHost.Instance.Config.Get(resourcePath).Send(async context =>
            {
                context.Response.StatusCode = 200;
                string responseContent = "Ok";
                byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseContent);
                buffer = System.Text.Encoding.UTF8.GetBytes(responseContent);
                await context.Response.Body.WriteAsync(buffer, 0, buffer.Length);
            });

            var response = await this.resilientHttpClient.GetAsync(string.Concat(this.defaultUrl, resourcePath));

            string responseData = await response.Content.ReadAsStringAsync();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task ResilientHttpClient_Get_WithHeader_Pass()
        {
            string resourcePath = "/resilient-get-with-header";

            InMemoryWebHost.Instance.Config.Get(resourcePath).Send(async context =>
            {
                string responseContent = string.Empty;

                if (context.Request.Headers.ContainsKey("Ocp-Apim-Subscription-Key"))
                {
                    context.Response.StatusCode = 200;
                    responseContent = "Ok";
                }
                else
                {
                    context.Response.StatusCode = 400;
                    responseContent = "Not Ok";
                }

                byte[] buffer = Encoding.UTF8.GetBytes(responseContent);
                buffer = Encoding.UTF8.GetBytes(responseContent);
                await context.Response.Body.WriteAsync(buffer, 0, buffer.Length);
            });

            Dictionary<string, string> keyValuePairs = new Dictionary<string, string>() { { "Ocp-Apim-Subscription-Key", Guid.NewGuid().ToString() } };
            var response = await this.resilientHttpClient.GetAsync(string.Concat(this.defaultUrl, resourcePath), "fake-token", "Bearer", keyValuePairs);

            string responseData = await response.Content.ReadAsStringAsync();

            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task ResilientHttpClient_Get_FailWithHttpRequestException()
        {
            var retryCount = 0;
            string resourcePath = "/resilient-get-exception";

            InMemoryWebHost.Instance.Config.Get(resourcePath).Send(context =>
            {
                retryCount++;
                context.Response.StatusCode = 500;
            });

            await Assert.ThrowsAsync<HttpRequestException>(async () => await this.resilientHttpClient.GetAsync(string.Format("{0}{1}", this.defaultUrl, resourcePath)));
            Assert.True(retryCount != 0);
        }

        [Fact]
        public async Task ResilientHttpClient_Post_UnableToDeserialize_FailWithHttpRequestException()
        {
            string resourcePath = "/resilient-post";

            string bodyContent;
            Product productToCreate;
            InMemoryWebHost.Instance.Config.Post(resourcePath).Send(async context =>
            {
                this.retryCount++;
                using (StreamReader reader = new StreamReader(context.Request.Body, Encoding.UTF8, true, 1024, true))
                {
                    bodyContent = reader.ReadToEnd();
                    try
                    {
                        productToCreate = JsonConvert.DeserializeObject<Product>(bodyContent);
                    }
                    catch (Exception ex)
                    {
                        context.Response.StatusCode = 500;
                        string responseContent = ex.Message;
                        byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseContent);
                        buffer = System.Text.Encoding.UTF8.GetBytes(responseContent);
                        await context.Response.Body.WriteAsync(buffer, 0, buffer.Length);
                    }
                }
            });

            await Assert.ThrowsAsync<HttpRequestException>(async () => await this.resilientHttpClient.PostAsync(string.Format("{0}{1}", this.defaultUrl, resourcePath), "{'data':'Test'}"));
            Assert.True(this.retryCount != 0);
        }
    }
}
