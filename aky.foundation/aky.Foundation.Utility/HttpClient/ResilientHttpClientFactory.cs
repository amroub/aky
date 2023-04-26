namespace aky.Foundation.Utility.HttpClient
{
    using System;
    using System.Net.Http;
    using aky.Foundation.Utility.Serialization;
    using Polly;

    public class ResilientHttpClientFactory : IResilientHttpClientFactory
    {
        private readonly int retryCount;
        private readonly ISerializer serializer;

        public ResilientHttpClientFactory(ISerializer serializer, int retryCount = 6)
        {
            this.serializer = serializer;
            this.retryCount = retryCount;
        }

        public ResilientHttpClient CreateResilientHttpClient() => new ResilientHttpClient(this.serializer, this.CreatePolicy());

        private Policy CreatePolicy() =>
                    Policy
                    .Handle<HttpRequestException>()
                    .WaitAndRetryAsync(this.retryCount, i => TimeSpan.FromSeconds(2), (Exception exception, TimeSpan time) => { });
    }
}
