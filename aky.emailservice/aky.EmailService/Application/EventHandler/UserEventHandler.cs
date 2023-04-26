namespace aky.EmailService.Application.EventHandler
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using aky.EmailService.Application.Event;
    using aky.EmailService.Domain.Services;
    using aky.EmailService.Infrastructure.EmailDispatcher;
    using aky.EmailService.Infrastructure.PlatformSetting;
    using aky.EmailService.TemplateEngine;
    using aky.Foundation.Ddd.Handlers;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;

    public class UserEventHandler : IHandles<ForgotPasswordEvent>
    {
        private readonly ITemplateService templateService;
        private readonly ITemplateEngine templateEngine;
        private readonly IEmailDispatcher emailDispatcher;
        private readonly IConfiguration configuration;
        private readonly ILogger<UserEventHandler> logger;
        private readonly IPlatformSettingProxy platformSettingProxy;

        public UserEventHandler(
            ITemplateService templateService,
            ITemplateEngine templateEngine,
            IEmailDispatcher emailDispatcher,
            IConfiguration configuration,
            ILogger<UserEventHandler> logger,
            IPlatformSettingProxy platformSettingProxy)
        {
            this.templateService = templateService;
            this.templateEngine = templateEngine;
            this.emailDispatcher = emailDispatcher;
            this.configuration = configuration;
            this.logger = logger;
            this.platformSettingProxy = platformSettingProxy;
        }

        public async Task HandleAsync(ForgotPasswordEvent message)
        {
            string adminEmail = this.configuration["adminEmail"];
            string storageAccountUrl = this.platformSettingProxy.GetConfigurationValue(ConfigResource.AzureStorageAccount, ConfigResourceSetting.StorageAccountEndpoint);

            var template = await this.templateService.GetTemplateByCode(nameof(ForgotPasswordEvent), message.LanguageCode);

            if (template == null)
            {
                this.logger.LogInformation($"There is no email template available for event: {message.GetType().Name}.");
                return;
            }

            try
            {
                string templateContent = string.Empty;
                using (HttpClient client = new HttpClient())
                {
                    templateContent = await client.GetStringAsync($"{storageAccountUrl}/{template.TemplatePath}");
                }

                string body = this.templateEngine.Prepare(templateContent, message);

                var emailSent = await this.emailDispatcher.SendAsync(new Email(adminEmail), new Email(message.Email), template.Subject, body);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, $"Error while sending email for event {message.GetType().Name}");
            }
        }
    }
}