namespace aky.EmailService.Application.Event
{
    using aky.Foundation.Ddd.Infrastructure;

    public class ForgotPasswordEvent : DomainEvent
    {
        public string UserId { get; set; }

        public string Email { get; set; }

        public string ValidationHash { get; set; }

        public string LanguageCode { get; set; }

        public string ResetPasswordLink { get; set; }
    }
}
