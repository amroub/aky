namespace Diatly.Foundation.Test.Diatly.Foundation.UtilityTest
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;
    using global::Diatly.Foundation.Test.Domain;
    using global::Diatly.Foundation.Utility.HttpClient;
    using Newtonsoft.Json;
    using Xunit;

    public class StandardHttpClientTest : BaseFixture
    {
        private string defaultUrl = "http://localhost:5000";
        private IHttpClient standardHttpClient;

        public StandardHttpClientTest()
        {
            this.standardHttpClient = this.StandardHttpClient;
        }

        [Fact]
        public async Task StandardHttpClient_Get_Pass()
        {
            string resourcePath = "/standard-get";

            InMemoryWebHost.Instance.Config.Get(resourcePath).Send(async context =>
            {
                context.Response.StatusCode = 200;
                string responseContent = "Ok";
                byte[] buffer = Encoding.UTF8.GetBytes(responseContent);
                buffer = Encoding.UTF8.GetBytes(responseContent);
                await context.Response.Body.WriteAsync(buffer, 0, buffer.Length);
            });

            var response = await this.standardHttpClient.GetAsync(string.Concat(this.defaultUrl, resourcePath));

            string responseData = await response.Content.ReadAsStringAsync();

            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task StandardHttpClient_Get_WithHeader_Pass()
        {
            string resourcePath = "/standard-get-with-header";

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
            var response = await this.standardHttpClient.GetAsync(string.Concat(this.defaultUrl, resourcePath), "fake-token", "Bearer", keyValuePairs);

            string responseData = await response.Content.ReadAsStringAsync();

            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        }

        [Theory]
        [MemberData(nameof(SingleProduct))]
        public async Task StandardHttpClient_Post_Pass(Product product)
        {
            string resourcePath = "/standard-post";

            string bodyContent;
            Product productToCreate;
            InMemoryWebHost.Instance.Config.Post(resourcePath).Send(async context =>
            {
                using (StreamReader reader = new StreamReader(context.Request.Body, Encoding.UTF8, true, 1024, true))
                {
                    bodyContent = reader.ReadToEnd();
                    productToCreate = JsonConvert.DeserializeObject<Product>(bodyContent);
                }

                context.Response.StatusCode = 200;
                string responseContent = string.Format("{0}-{1}", productToCreate.ProductName, "Created");
                byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseContent);
                buffer = System.Text.Encoding.UTF8.GetBytes(responseContent);
                await context.Response.Body.WriteAsync(buffer, 0, buffer.Length);
            });

            var httpResponse = await this.standardHttpClient.PostAsync(string.Concat(this.defaultUrl, resourcePath), product);

            string responseData = await httpResponse.Content.ReadAsStringAsync();

            Assert.Equal(System.Net.HttpStatusCode.OK, httpResponse.StatusCode);
            Assert.Equal(string.Format("{0}-{1}", product.ProductName, "Created"), responseData);
        }

        [Theory]
        [MemberData(nameof(SingleProduct))]
        public async Task StandardHttpClient_Put_Pass(Product product)
        {
            string resourcePath = "/standard-put/123";

            string bodyContent;
            Product productToCreate;
            InMemoryWebHost.Instance.Config.Put(resourcePath).Send(async context =>
            {
                using (StreamReader reader = new StreamReader(context.Request.Body, Encoding.UTF8, true, 1024, true))
                {
                    bodyContent = reader.ReadToEnd();
                    productToCreate = JsonConvert.DeserializeObject<Product>(bodyContent);
                }

                context.Response.StatusCode = 200;
                string responseContent = string.Format(CultureInfo.InvariantCulture, "{0}-{1}", productToCreate.ProductName, "Updated");
                byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseContent);
                buffer = System.Text.Encoding.UTF8.GetBytes(responseContent);
                await context.Response.Body.WriteAsync(buffer, 0, buffer.Length);
            });

            var httpResponse = await this.standardHttpClient.PutAsync(string.Concat(this.defaultUrl, resourcePath), product);

            string responseData = await httpResponse.Content.ReadAsStringAsync();

            Assert.Equal(System.Net.HttpStatusCode.OK, httpResponse.StatusCode);
            Assert.Equal(string.Format(CultureInfo.InvariantCulture, "{0}-{1}", product.ProductName, "Updated"), responseData);
        }

        [Fact]
        public async Task StandardHttpClient_Delete_Pass()
        {
            string resourcePath = "/standard-delete/123";

            InMemoryWebHost.Instance.Config.Delete(resourcePath).Send("Deleted", 200);

            var httpResponse = await this.standardHttpClient.DeleteAsync(string.Concat(this.defaultUrl, resourcePath));

            string responseData = await httpResponse.Content.ReadAsStringAsync();

            Assert.Equal(System.Net.HttpStatusCode.OK, httpResponse.StatusCode);
            Assert.Equal("Deleted", responseData);
        }

        public static IEnumerable<object[]> SingleProduct
        {
            get
            {
                yield return new Product[]
                {
                new Product()
                {
                    ProductName = "This is test product",
                    Description = "Étagère avec structure en bois d'acacia massif et plateaux en ciment. Pour usage intérieur et extérieur à l'abri ou couvert.",
                     Category = new Category() { Name = "Test category" },
                    AvailableStock = 0,
                    BrandID = 1701,
                    Price = 22800.00M,
                },
                };
            }
        }
    }
}
