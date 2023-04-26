namespace aky.EmailService
{
    using System;
    using System.Linq;
    using System.Reflection;
    using Autofac;
    using Autofac.Extensions.DependencyInjection;
    using Autofac.Extras.CommonServiceLocator;
    using aky.EmailService.Application.Event;
    using aky.EmailService.DI;
    using aky.EmailService.Infrastructure;
    using aky.Foundation.AzureServiceBus;
    using aky.Foundation.Ddd.Handlers;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Azure.ServiceBus;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Ordering.Infrastructure;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public IContainer ApplicationContainer { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<ISubscriptionInvocationManager, SubscriptionInvocationManager>();

            services.AddMvc();

            var builder = new ContainerBuilder();

            var assemby = Assembly.GetExecutingAssembly();

            builder.RegisterAssemblyTypes(assemby)
                   .Where(x =>
                   {
                       var allInterfaces = x.GetInterfaces();
                       return
                           allInterfaces.Any(y => y.GetTypeInfo().IsGenericType && y.GetTypeInfo().GetGenericTypeDefinition() == typeof(IHandles<>));
                   })
                   .AsImplementedInterfaces();

            builder.RegisterModule(new akyAutofactModule());

            builder.Populate(services);

            this.ApplicationContainer = builder.Build();

            var csl = new AutofacServiceLocator(this.ApplicationContainer);

            CommonServiceLocator.ServiceLocator.SetLocatorProvider(() => csl);

            return new AutofacServiceProvider(this.ApplicationContainer);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app,
            IHostingEnvironment env,
            ISubscriber subscriber,
            ISubscriptionInvocationManager subscriptionInvocationManager,
            ILogger<Startup> logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            try
            {
                subscriber.Subscribe<ForgotPasswordEvent>("forgotpassword", subscriptionInvocationManager.HandleEvents);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error while subscribing service bus event");
            }

            app.UseMvc();
        }
    }
}
