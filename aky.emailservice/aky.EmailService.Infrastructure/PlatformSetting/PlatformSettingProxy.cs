namespace aky.EmailService.Infrastructure.PlatformSetting
{
    using aky.Foundation.Utility;
    using System.Linq;

    public class PlatformSettingProxy : IPlatformSettingProxy
    {
        private IPlatformSettingService platformSettingService;

        public PlatformSettingProxy(IPlatformSettingService platformSettingService)
        {
            this.platformSettingService = platformSettingService;
        }

        public string GetConfigurationValue(string resourceCode, string resourceKey)
        {
            string keyValue = null;

            var config = this.platformSettingService.GetConfiguration("Resource", resourceCode, "PLATFORM_SETTINGS_API").Result;
            if (config != null)
            {
                var configSettings = config.ResourceSettings.FirstOrDefault(x => x.Key == resourceKey);
                keyValue = configSettings?.Value;
            }

            return keyValue;
        }
    }
}
