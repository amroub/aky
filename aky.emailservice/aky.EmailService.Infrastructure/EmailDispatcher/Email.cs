namespace aky.EmailService.Infrastructure.EmailDispatcher
{
    public class Email
    {
        public Email()
        {
        }

        public Email(string emailAddress, string name = null)
        {
            this.EmailAddress = emailAddress;
            this.Name = name;
        }

        public string Name { get; set; }

        public string EmailAddress { get; set; }
    }
}
