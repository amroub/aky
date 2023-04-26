namespace aky.EmailService.Infrastructure.EmailDispatcher
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class NullDispatcher : IEmailDispatcher
    {
        public Task<bool> SendAsync(Email sender, Email receiver, string subject, string body)
        {
            return Task.FromResult(true);
        }

        public Task<bool> SendAsync(Email sender, Email receiver, List<Email> carbonCopyEmailAddress, string subject, string body)
        {
            return Task.FromResult(true);
        }
    }
}
