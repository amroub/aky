namespace aky.EmailService.Infrastructure.PlatformSetting
{
    public class ConfigResource
    {
        public static string SendGrid_Email_Client = "sendGrid-emailClient";

        public static string IdentityTopic = "IdentityTopic";

        public static string EmailServiceDb_ConnectionString = "EmailServiceDb-ConnectionString";

        public static string AzureStorageAccount = "AzureStorageAccount";

        public string ResourceCode { get; set; }

        public ConfigResourceSetting[] ResourceSettings { get; set; }
    }
}
