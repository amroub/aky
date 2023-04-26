namespace aky.EmailService.DI
{
    using System.Linq;
    using System.Reflection;
    using Autofac;
    using aky.EmailService.Domain.Repositories;
    using aky.EmailService.Domain.Services;
    using aky.EmailService.Infrastructure.EmailDispatcher;
    using aky.EmailService.Infrastructure.PlatformSetting;
    using aky.EmailService.Infrastructure.Repositories;
    using aky.EmailService.Infrastructure.Services;
    using aky.EmailService.TemplateEngine;
    using aky.Foundation.AzureServiceBus;
    using aky.Foundation.Ddd.Handlers;
    using aky.Foundation.Ddd.Infrastructure;
    using aky.Foundation.Utility;
    using Microsoft.Azure.ServiceBus;
    using Microsoft.EntityFrameworkCore;
    using Ordering.Infrastructure;
    using SendGrid;

    public class akyAutofactModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            this.AddFoundations(builder);
            this.AddUtilities(builder);
            this.AddInfrastructure(builder);
            this.AddDomainRepositories(builder);
            this.AddDomainServices(builder);
        }

        private void AddFoundations(ContainerBuilder builder)
        {
            builder.RegisterType<Mediator>().As<IMediator>().SingleInstance();
            builder.RegisterType<Subscriber>().As<ISubscriber>().InstancePerDependency();
            builder.RegisterUtilities();
            builder.RegisterResilientHttpClients();
            builder.RegisterPlatformSetting();

            builder.RegisterType<PlatformSettingProxy>().As<IPlatformSettingProxy>().SingleInstance();
        }

        private void AddDomainRepositories(ContainerBuilder builder)
        {
            // register first DbContext
            builder.Register(a =>
            {
                var platformSettingProxy = a.Resolve<IPlatformSettingProxy>();

                var connectionString = platformSettingProxy.GetConfigurationValue(ConfigResource.EmailServiceDb_ConnectionString, ConfigResourceSetting.DbConnectionString);

                connectionString = connectionString.Replace(@"\\", @"\");

                return new DbContextOptionsBuilder<EmailDbContext>()
                                       .UseSqlServer(connectionString)
                                       .Options;
            }).SingleInstance();
            builder.RegisterType<EmailDbContext>().AsSelf().InstancePerLifetimeScope();

            // register all repositories
            builder.RegisterType<TemplateRepository>().As<ITemplateRepository>().InstancePerDependency();
        }

        private void AddDomainServices(ContainerBuilder builder)
        {
            builder.RegisterType<TemplateService>().As<ITemplateService>().InstancePerDependency();
        }

        private void AddUtilities(ContainerBuilder builder)
        {
            // email template engine injections
            builder.RegisterType<DotLiquidTemplateEngine>().As<ITemplateEngine>().InstancePerDependency();
            builder.RegisterType<SendGridDispatcher>().As<IEmailDispatcher>().SingleInstance();

            // sendgrid injections
            builder.Register(a =>
            {
                var platformSettingProxy = a.Resolve<IPlatformSettingProxy>();
                var sendGridKey = platformSettingProxy.GetConfigurationValue(ConfigResource.SendGrid_Email_Client, ConfigResourceSetting.SendGrid_Email_Client_Api_Key);

                return new SendGridClient(sendGridKey);
            }).
            As<ISendGridClient>().SingleInstance();
        }

        private void AddInfrastructure(ContainerBuilder builder)
        {
            // register azure service bus
            builder.Register(a =>
            {
                var platformSettingProxy = a.Resolve<IPlatformSettingProxy>();
                var servicebusConnection = new ServiceBusConnectionStringBuilder(platformSettingProxy.GetConfigurationValue(ConfigResource.IdentityTopic, ConfigResourceSetting.IdentityTopic_EventBusConStr));

                return new DefaultServiceBusPersisterConnection(servicebusConnection);
            }).As<IServiceBusPersisterConnection>().SingleInstance();
        }
    }
}
