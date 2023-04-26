namespace aky.EmailService.Tests
{
    using System;
    using Autofac;
    using Autofac.Extras.CommonServiceLocator;
    using aky.EmailService.DI;
    using aky.EmailService.Domain.Repositories;
    using aky.EmailService.TemplateEngine;
    using Microsoft.EntityFrameworkCore;
    using Ordering.Infrastructure;

    public abstract class BaseFixture : IDisposable
    {
        private bool disposed = false;

        private IContainer autofacContainer;

        protected IContainer AutofacContainer
        {
            get
            {
                if (this.autofacContainer == null)
                {
                    var builder = new ContainerBuilder();

                    builder.RegisterModule(new akyAutofactModule());

                    this.RegisterTestInjections(builder);

                    var container = builder.Build();

                    var csl = new AutofacServiceLocator(container);

                    CommonServiceLocator.ServiceLocator.SetLocatorProvider(() => csl);

                    this.autofacContainer = container;
                }

                return this.autofacContainer;
            }
        }

        protected ITemplateRepository TemplateRepository => AutofacContainer.Resolve<ITemplateRepository>();

        protected ITemplateEngine TemplateEngine => AutofacContainer.Resolve<ITemplateEngine>();

        public void Dispose()
        {
            // Dispose of unmanaged resources.
            this.Dispose(true);

            // Suppress finalization.
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (this.disposed)
            {
                return;
            }

            if (disposing)
            {
                if (this.autofacContainer != null)
                {
                    this.autofacContainer.Dispose();
                }
            }

            this.disposed = true;
        }

        private void RegisterTestInjections(ContainerBuilder containerBuilder)
        {
            containerBuilder.Register(a => new DbContextOptionsBuilder<EmailDbContext>().UseInMemoryDatabase(databaseName: "emailServiceDb").Options)
                    .SingleInstance();
            containerBuilder.RegisterType<EmailDbContext>().AsSelf().InstancePerDependency();
        }
    }
}
