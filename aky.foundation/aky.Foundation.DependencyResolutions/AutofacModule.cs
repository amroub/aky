namespace aky.Foundation.DependencyResolutions
{
    using Autofac;
    using aky.Foundation.AzureServiceBus;
    using aky.Foundation.Ddd.Infrastructure;
    using aky.Foundation.Utility.HttpClient;
    using aky.Foundation.Utility.Serialization;

    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<Mediator>().As<IMediator>().SingleInstance();

            builder.RegisterType<Publisher>().As<IPublisher>().InstancePerDependency();
            builder.RegisterType<Subscriber>().As<ISubscriber>().InstancePerDependency();

            this.AddUtilities(builder);
        }

        private void AddUtilities(ContainerBuilder builder)
        {
            builder.RegisterType<JsonSerializer>().As<ISerializer>().SingleInstance();
            builder.RegisterType<StandardHttpClient>().As<IHttpClient>().SingleInstance();
            builder.Register(a =>
            {
                var serializer = a.Resolve<ISerializer>();
                return new ResilientHttpClientFactory(serializer);
            })
            .As<IResilientHttpClientFactory>().SingleInstance();

            builder.Register(a => a.Resolve<IResilientHttpClientFactory>().CreateResilientHttpClient()).As<IHttpClient>().SingleInstance();
        }
    }
}
