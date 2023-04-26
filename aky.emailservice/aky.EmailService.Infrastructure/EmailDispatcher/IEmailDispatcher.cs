namespace aky.EmailService.Infrastructure.EmailDispatcher
{
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Mail;
    using System.Threading.Tasks;

    public interface IEmailDispatcher
    {
        Task<bool> SendAsync(Email sender, Email receiver, string subject, string body);

        Task<bool> SendAsync(Email sender, Email receiver, List<Email> carbonCopyEmailAddress, string subject, string body);
    }
}
