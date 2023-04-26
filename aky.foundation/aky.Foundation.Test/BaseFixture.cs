namespace Diatly.Foundation.Test
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Xml;
    using Autofac;
    using Autofac.Extras.CommonServiceLocator;
    using global::Akeneo;
    using global::Akeneo.Authentication;
    using global::Diatly.Foundation.Ddd.Handlers;
    using global::Diatly.Foundation.Ddd.Infrastructure;
    using global::Diatly.Foundation.DependencyResolutions;
    using global::Diatly.Foundation.Test.Diatly.Foundation.Ddd.CommandHandler;
    using global::Diatly.Foundation.Test.Diatly.Foundation.Ddd.Commands;
    using global::Diatly.Foundation.Test.Diatly.Foundation.UtilityTest;
    using global::Diatly.Foundation.Test.Domain;
    using global::Diatly.Foundation.Test.Repository;
    using global::Diatly.Foundation.Test.Repository.Interfaces;
    using global::Diatly.Foundation.Utility.HttpClient;
    using global::Diatly.Foundation.Utility.Logging;
    using global::Diatly.Foundation.Utility.Logging.ApplicationInsights;
    using global::Diatly.Foundation.Utility.Logging.Log4Net;
    using global::Diatly.Foundation.Utility.Serialization;
    using Microsoft.ApplicationInsights;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.EntityFrameworkCore;

    public abstract class BaseFixture : IDisposable
    {
        public AkeneoClient Client { get; set; }

        private IContainer autofacContainer;

        private void AddAkeneo(ContainerBuilder builder)
        {
            var akeneoTestEnvironment = new Uri("http://40.127.206.144");
            var clientId = "1_3xo997tqbm0wwcokwoc044w8gso808cs4g0gc0c8w8cso4kc0g";
            var clientSecret = "4oe1o889vrsw8ww4owsowsg0gc8gckwgokgk0cc4s0sc04scgo";
            var userName = "admin";
            var password = "Diatly@2018";

            builder.Register(a => new Authenticator(akeneoTestEnvironment, clientId, clientSecret, userName, password)).
                As<IAuthenticator>();

            builder.Register(a =>
            {
                var authenticator = a.Resolve<IAuthenticator>();
                return new AkeneoClient(akeneoTestEnvironment, authenticator);
            }).As<IAkeneoClient>();
        }

        protected IContainer AutofacContainer
        {
            get
            {
                if (this.autofacContainer == null)
                {
                    var builder = new ContainerBuilder();

                    builder.RegisterModule(new AutofacModule());

                    builder.Register(a => new DbContextOptionsBuilder<CatalogContext>().UseInMemoryDatabase(databaseName: "DiatlyInMemoryTestDB").Options)
                    .SingleInstance();

                    builder.RegisterType<CatalogContext>().AsSelf().InstancePerDependency();
                    builder.RegisterType<ProductRepository>().As<IProductRepository>().InstancePerDependency();
                    builder.RegisterType<TemplateRepository>().As<ITemplateRepository>().InstancePerDependency();
                    builder.RegisterType<TestCommandHandler>().As<IHandles<TestCommand>>().InstancePerDependency();
                    builder.Register(a =>
                    {
                        XmlDocument log4netConfig = new XmlDocument();
                        log4netConfig.Load(File.OpenRead("log4net.xml"));

                        return new Log4NetLogger("log4Net", log4netConfig["log4net"]);
                    })
                    .As<IDiatlyLogger>();

                    builder.Register(a =>
                    {
                        var fakeChannel = new FakeTelemetryChannel();
                        var config = new TelemetryConfiguration
                        {
                            TelemetryChannel = fakeChannel,
                            InstrumentationKey = "test key",
                        };
                        var client = new TelemetryClient(config);

                        return new ApplicationInsightsLogger("log4Net", client, null, new ApplicationInsightsSettings() { DeveloperMode = true, InstrumentationKey = "test key" } );
                    })
                    .As<IDiatlyLogger>();

                    // Add akeneo related injections.
                    this.AddAkeneo(builder);

                    var container = builder.Build();

                    var csl = new AutofacServiceLocator(container);

                    CommonServiceLocator.ServiceLocator.SetLocatorProvider(() => csl);

                    this.autofacContainer = container;
                }

                return this.autofacContainer;
            }
        }

        protected IProductRepository ProductRepository => AutofacContainer.Resolve<IProductRepository>();

        protected ITemplateRepository TemplateRepository => AutofacContainer.Resolve<ITemplateRepository>();

        protected IHttpClient StandardHttpClient
        {
            get
            {
                IHttpClient resilientClient = null;

                var clients = this.AutofacContainer.Resolve<IEnumerable<IHttpClient>>();

                foreach (IHttpClient client in clients)
                {
                    if (client is StandardHttpClient)
                    {
                        resilientClient = client;
                        break;
                    }
                }

                return resilientClient;
            }
        }

        protected IHttpClient ResilientHttpClient
        {
            get
            {
                IHttpClient resilientClient = null;

                var clients = this.AutofacContainer.Resolve<IEnumerable<IHttpClient>>();

                foreach (IHttpClient client in clients)
                {
                    if (client is ResilientHttpClient)
                    {
                        resilientClient = client;
                        break;
                    }
                }

                return resilientClient;
            }
        }

        protected IMediator Mediator => AutofacContainer.Resolve<IMediator>();

        protected IDiatlyLogger Log4NetLogger
        {
            get
            {
                IDiatlyLogger log4NetLogger = null;

                var loggers = this.AutofacContainer.Resolve<IEnumerable<IDiatlyLogger>>();

                foreach (IDiatlyLogger logger in loggers)
                {
                    if (logger is Log4NetLogger)
                    {
                        log4NetLogger = logger;
                        break;
                    }
                }

                return log4NetLogger;
            }
        }

        protected IDiatlyLogger ApplicationInsightsLogger
        {
            get
            {
                IDiatlyLogger applicationInsightsLogger = null;

                var loggers = this.AutofacContainer.Resolve<IEnumerable<IDiatlyLogger>>();

                foreach (IDiatlyLogger logger in loggers)
                {
                    if (logger is ApplicationInsightsLogger)
                    {
                        applicationInsightsLogger = logger;
                        break;
                    }
                }

                return applicationInsightsLogger;
            }
        }

        protected ISerializer Serializer => AutofacContainer.Resolve<ISerializer>();

        protected T Resolve<T>()
            where T : class
        {
            var type = this.AutofacContainer.Resolve<T>();
            return type;
        }

        public void Dispose()
        {
            Client?.Dispose();

            if (this.autofacContainer != null)
            {
                this.autofacContainer.Dispose();
            }
        }
    }
}
