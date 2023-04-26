namespace Diatly.Foundation.Test.Akeneo
{
    using System.Threading.Tasks;
    using global::Akeneo;
    using global::Akeneo.Model;
    using Xunit;

    public class ChannelTests : BaseFixture
    {
        private IAkeneoClient akeneoClient;

        public ChannelTests()
        {
            this.akeneoClient = this.Resolve<IAkeneoClient>();
        }

        [Fact]
        public async Task GetChannelsAsync()
        {
            var channels = await this.akeneoClient.GetManyAsync<Channel>();

            // Assert
            Assert.NotNull(channels);
            Assert.True(channels.Embedded.Items.Count > 0);
        }
    }
}
