using System.Threading;
using System.Threading.Tasks;

namespace Akeneo.Authentication
{
    public interface IAuthenticator
    {
        Task<TokenResponse> RequestAccessTokenAsync();
        Task<TokenResponse> RequestRefreshTokenAsync(string refreshToken);
        Task<TokenResponse> GetAccessTokenAsync();
    }
}
