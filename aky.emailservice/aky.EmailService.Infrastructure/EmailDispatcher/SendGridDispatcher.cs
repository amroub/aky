namespace aky.EmailService.Infrastructure.EmailDispatcher
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using SendGrid;
    using SendGrid.Helpers.Mail;

    public class SendGridDispatcher : IEmailDispatcher
    {
        private ISendGridClient sendGridClient;

        public SendGridDispatcher(ISendGridClient sendGridClient)
        {
            this.sendGridClient = sendGridClient;
        }

        public async Task<bool> SendAsync(Email sender, Email receiver, string subject, string body)
        {
            var response = await this.SendEmail(sender, receiver, null, subject, body);

            return response.StatusCode == System.Net.HttpStatusCode.OK;
        }

        public async Task<bool> SendAsync(Email sender, Email receiver, List<Email> carbonCopyEmailAddress, string subject, string body)
        {
            var response = await this.SendEmail(sender, receiver, carbonCopyEmailAddress, subject, body);

            return response.StatusCode == System.Net.HttpStatusCode.OK;
        }

        private async Task<Response> SendEmail(Email sender, Email receiver, List<Email> carbonCopyEmailAddress, string subject, string body)
        {
            var msg = new SendGridMessage()
            {
                From = new SendGrid.Helpers.Mail.EmailAddress(sender.EmailAddress, sender.Name),
                Subject = subject,
                HtmlContent = body,
            };

            msg.AddTo(new SendGrid.Helpers.Mail.EmailAddress(receiver.EmailAddress, receiver.Name));

            if (carbonCopyEmailAddress != null && carbonCopyEmailAddress.Any())
            {
                msg.AddCcs(carbonCopyEmailAddress.
                            Select(a => new SendGrid.Helpers.Mail.EmailAddress(a.EmailAddress, a.Name)).
                            ToList());
            }

            var response = await this.sendGridClient.SendEmailAsync(msg);

            return response;
        }
    }
}
