namespace aky.Foundation.Utility
{
    using Autofac;
    using aky.Foundation.Utility.HttpClient;
    using aky.Foundation.Utility.Serialization;
    using Microsoft.Extensions.Configuration;

    public static class DIHelper
    {
        public static void RegisterUtilities(this ContainerBuilder builder)
        {
            // register other utilities
            builder.RegisterType<JsonSerializer>().As<ISerializer>().SingleInstance();
        }

        public static void RegisterResilientHttpClients(this ContainerBuilder builder)
        {
            // register resilient http client
            builder.Register(a =>
            {
                var serializer = a.Resolve<ISerializer>();
                return new ResilientHttpClientFactory(serializer);
            })
            .As<IResilientHttpClientFactory>().SingleInstance();

            builder.Register(a => a.Resolve<IResilientHttpClientFactory>().CreateResilientHttpClient()).As<IHttpClient>().SingleInstance();
        }

        public static void RegisterStandardHttpClients(this ContainerBuilder builder)
        {
            // register standard http client
            builder.RegisterType<StandardHttpClient>().As<IHttpClient>().SingleInstance();
        }

        public static void RegisterPlatformSetting(this ContainerBuilder builder)
        {
            // register platform setting service
            builder.Register(a =>
            {
                IConfiguration configuration = a.Resolve<IConfiguration>();
                var identityClientConfig = new PlatformSettingIdentityClient();

                var identityConfigSection = configuration.GetSection("PlatformSettingIdentityClient");
                identityConfigSection.Bind(identityClientConfig);

                return new PlatformSettingService(identityClientConfig, a.Resolve<IHttpClient>(), a.Resolve<ISerializer>());
            }).
            As<IPlatformSettingService>().SingleInstance();
        }
    }
}
