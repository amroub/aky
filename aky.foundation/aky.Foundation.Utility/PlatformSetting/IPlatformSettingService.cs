namespace aky.Foundation.Utility
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IPlatformSettingService
    {
        Task<Resource> GetConfiguration(string resource, string resourceCode, string scope = "PLATFORM_SETTINGS_API", Dictionary<string, string> customHeaders = null);
    }
}
