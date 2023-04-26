namespace aky.EmailService.Infrastructure.PlatformSetting
{
    public interface IPlatformSettingProxy
    {
        string GetConfigurationValue(string resourceCode, string resourceKey);
    }
}
