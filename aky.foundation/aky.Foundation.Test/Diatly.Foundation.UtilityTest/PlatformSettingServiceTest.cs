namespace Diatly.Foundation.Test.Diatly.Foundation.UtilityTest
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using global::Diatly.Foundation.Utility;
    using Moq;
    using Xunit;

    public class PlatformSettingServiceTest : BaseFixture
    {
        public PlatformSettingServiceTest()
        {
        }

        [Fact]
        public async Task PlatformSettingService_GetConfiguration_Pass()
        {
            // Arrage
            var platformSettingService = new Mock<IPlatformSettingService>();
            platformSettingService.
                Setup(a => a.GetConfiguration(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, string>>())).
                ReturnsAsync(new Resource()
                {
                    ResourceCode = "sendGrid-emailClient",
                    ResourceSettings = new ResourceSetting[] { new ResourceSetting() { Key = "EmailClientApiKey", Value = "tempValue" } },
                });

            // Act
            var resource = await platformSettingService.Object.GetConfiguration(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>());

            // Assert
            Assert.NotNull(resource);
        }

        [Fact]
        public async Task PlatformSettingService_GetConfiguration_ResourceDontExists()
        {
            // Arrage
            Resource resource = null;
            var platformSettingService = new Mock<IPlatformSettingService>();
            platformSettingService.
                Setup(a => a.GetConfiguration(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, string>>())).
                ReturnsAsync(resource);

            // Act
            var configurationResource = await platformSettingService.Object.GetConfiguration(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>());

            // Assert
            Assert.Null(configurationResource);
        }

        //[Fact]
        //public async Task Test()
        //{
        //    // Arrage
        //    PlatformSettingIdentityClient platformSettingIdentityClient = new PlatformSettingIdentityClient()
        //    {
        //        IdentityUrl = "https://auth.diatly.com",
        //        ClientId = "internal.platformSettingService.client",
        //        ClientSecret = "gMZ3jRltL3HE0yZL",
        //        PlatformSettingServiceUrl = "https://api.diatly.com/v1/platformsettings",
        //        OcpApimSubscriptionKey = "813bb793adf44698a88d9fe218301263",
        //    };
        //    IPlatformSettingService platformSettingService = new PlatformSettingService(platformSettingIdentityClient, this.ResilientHttpClient, this.Serializer);

        //    // Act
        //    var configurationResource = await platformSettingService.GetConfiguration("resource", "sendGrid-emailClient", "PLATFORM_SETTINGS_API");

        //    // Assert
        //    Assert.NotNull(configurationResource);
        //}
    }
}
