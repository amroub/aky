using Diatly.Foundation.AzureServiceBus;
using Diatly.Foundation.Ddd.Infrastructure;
using Microsoft.Azure.ServiceBus;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Diatly.Foundation.Test
{
    public class Test
    {
        [Fact]
        public void TestMethod()
        {
            var serviceBusConnection = new ServiceBusConnectionStringBuilder("Endpoint=sb://chikki-dev-servicebus.servicebus.windows.net/;SharedAccessKeyName=identitySAP;SharedAccessKey=BgAfrfPLFDfVqX7dnOU2/9qaVuELMFBHDzKRtRgISS0=;EntityPath=identity");

            IPublisher publisher = new Publisher(new DefaultServiceBusPersisterConnection(serviceBusConnection));

            ForgotPasswordEvent forgetPasswordEvent = new ForgotPasswordEvent()
            {
                Email = "jatinkacha@dhanashree.com",
                UserId = new Guid().ToString(),
                ValidationHash = new Guid().ToString(),
                LanguageCode = "Fr",
                ResetPasswordLink = "http://diatly.com",
            };

            try
            {
                publisher.Send<ForgotPasswordEvent>(forgetPasswordEvent);
            }
            catch(Exception ex)
            {

            }
        }
    }

    public class ForgotPasswordEvent : DomainEvent
    {
        public string UserId { get; set; }

        public string Email { get; set; }

        public string ValidationHash { get; set; }

        public string LanguageCode { get; set; }

        public string ResetPasswordLink { get; set; }
    }
}
