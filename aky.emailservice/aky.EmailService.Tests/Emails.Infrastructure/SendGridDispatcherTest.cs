namespace aky.EmailService.Tests.Emails.Infrastructure
{
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading;
    using System.Threading.Tasks;
    using aky.EmailService.Infrastructure.EmailDispatcher;
    using Moq;
    using SendGrid;
    using SendGrid.Helpers.Mail;
    using Xunit;

    public partial class SendGridDispatcherTest : BaseFixture
    {
        public SendGridDispatcherTest()
        {
        }

        [Fact]
        public async Task SendGridDispatcher_SendAsync_SimpleEMail()
        {
            Mock<ISendGridClient> sendGridClient = new Mock<ISendGridClient>();

            Response response = new Response(System.Net.HttpStatusCode.OK, It.IsAny<HttpContent>(), It.IsAny<HttpResponseHeaders>());

            sendGridClient.Setup(a => a.SendEmailAsync(It.IsAny<SendGridMessage>(), It.IsAny<CancellationToken>())).ReturnsAsync(response);

            IEmailDispatcher emailDispatcher = new SendGridDispatcher(sendGridClient.Object);

            var result = await emailDispatcher.SendAsync(
                new Email("no-reply@aky.com", "no-reply"),
                new Email("jatinkacha@dhanashree.com", "Jatin"),
                "test subject",
                "test body");

            Assert.True(result);
        }

        [Fact]
        public async Task SendGridDispatcher_SendAsync_WithWrongEmailAddress()
        {
            Mock<ISendGridClient> sendGridClient = new Mock<ISendGridClient>();

            Response response = new Response(System.Net.HttpStatusCode.BadRequest, It.IsAny<HttpContent>(), It.IsAny<HttpResponseHeaders>());

            sendGridClient.Setup(a => a.SendEmailAsync(It.IsAny<SendGridMessage>(), It.IsAny<CancellationToken>())).ReturnsAsync(response);

            IEmailDispatcher emailDispatcher = new SendGridDispatcher(sendGridClient.Object);

            var result = await emailDispatcher.SendAsync(
                new Email("no-reply@", "no-reply"),
                new Email("jatinkacha@dhanashree.com", "Jatin"),
                "test subject",
                "test body");

            Assert.False(result);
        }
    }
}
