namespace Diatly.Foundation.Test.Akeneo
{
    using System.Threading.Tasks;
    using global::Akeneo.Authentication;
    using Xunit;

    public class AuthenticationTest : BaseFixture
    {
        private IAuthenticator authenticator;

        public AuthenticationTest()
        {
            this.authenticator = this.Resolve<IAuthenticator>();
        }

        [Fact]
        public async Task RequestFreshAccessToken()
        {
            // setup
            var response = await this.authenticator.RequestAccessTokenAsync();

            // assert
            Assert.NotNull(response.AccessToken);
        }

        [Fact]
        public async Task GetAccessToken_MultipleAccess_GetsRefreshToken()
        {
            // setup
            var response = await this.authenticator.GetAccessTokenAsync();

            // assert
            Assert.NotNull(response.AccessToken);

            // get access token again
            var response2 = await this.authenticator.GetAccessTokenAsync();

            // assert
            Assert.NotNull(response2.AccessToken);
            Assert.NotEqual(response.AccessToken, response2.AccessToken);
        }
    }
}
