namespace aky.EmailService.Infrastructure.PlatformSetting
{
    public class ConfigResourceSetting
    {
        public static string SendGrid_Email_Client_Api_Key = "EmailClientApiKey";

        public static string IdentityTopic_EventBusConStr = "IdentityTopic-EventBusConStr";

        public static string DbConnectionString = "DbConnectionString";

        public static string StorageAccountEndpoint = "StorageAccountEndpoint";

        public string Key { get; set; }

        public string Value { get; set; }
    }
}
