namespace aky.Foundation.Utility
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;
    using aky.Foundation.Utility.HttpClient;
    using aky.Foundation.Utility.Serialization;
    using IdentityModel.Client;

    public class PlatformSettingService : IPlatformSettingService
    {
        private readonly PlatformSettingIdentityClient platformSettingIdentityClient;
        private readonly IHttpClient httpClient;
        private readonly ISerializer serializer;

        public PlatformSettingService(PlatformSettingIdentityClient platformSettingIdentityClient, IHttpClient httpClient, ISerializer serializer)
        {
            this.platformSettingIdentityClient = platformSettingIdentityClient;
            this.httpClient = httpClient;
            this.serializer = serializer;
        }

        public async Task<Resource> GetConfiguration(string resource, string resourceCode, string scope = "PLATFORM_SETTINGS_API", Dictionary<string, string> customHeaders = null)
        {
            string apimOcpKey = "Ocp-Apim-Subscription-Key";

            Resource configResource = null;

            var discoveryResponse = await DiscoveryClient.GetAsync(this.platformSettingIdentityClient.IdentityUrl);

            if (!discoveryResponse.IsError)
            {
                var tokenClient = new TokenClient(discoveryResponse.TokenEndpoint, this.platformSettingIdentityClient.ClientId, this.platformSettingIdentityClient.ClientSecret);
                var tokenResponse = await tokenClient.RequestClientCredentialsAsync(scope);

                if (!tokenResponse.IsError)
                {
                    var token = tokenResponse.AccessToken;

                    var uri = $"{this.platformSettingIdentityClient.PlatformSettingServiceUrl}/{resource}?resourceCode={resourceCode}";
                    try
                    {
                        if (customHeaders != null)
                        {
                            if (!customHeaders.ContainsKey(apimOcpKey))
                            {
                                customHeaders.Add(apimOcpKey, this.platformSettingIdentityClient.OcpApimSubscriptionKey);
                            }
                        }
                        else
                        {
                            customHeaders = new Dictionary<string, string> { { apimOcpKey, this.platformSettingIdentityClient.OcpApimSubscriptionKey } };
                        }

                        var response = await this.httpClient.GetAsync(uri, token, "Bearer", customHeaders);

                        using (StreamReader reader = new StreamReader(response.Content.ReadAsStreamAsync().Result, Encoding.UTF8, true, 1024, true))
                        {
                            string bodyContent = reader.ReadToEnd();
                            var rs = this.serializer.Deserialize<ResourceSetting[]>(bodyContent);

                            configResource = new Resource() { ResourceCode = resourceCode, ResourceSettings = rs };
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }

            return configResource;
        }
    }
}
