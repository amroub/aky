using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Akeneo.Client;
using Akeneo.Common;
using Akeneo.Http;

namespace Akeneo.Authentication
{
	public class Authenticator : IAuthenticator, IDisposable
	{
		private readonly string _username;
		private readonly string _password;
		private readonly HttpClient _httpClient;
		private TokenResponse _latestAccessToken;
		private const string BasicAuthHeader = "Basic";

		public Authenticator(Uri apiEndPoint, string clientId, string clientSecret, string username, string password)
		{
			_username = username;
			_password = password;
			_httpClient = new HttpClient
			{
				BaseAddress = apiEndPoint,
				DefaultRequestHeaders =
				{
					Authorization = new AuthenticationHeaderValue(BasicAuthHeader, CreateAuthHeader(clientId, clientSecret))
				}
			};
		}

		public virtual async Task<TokenResponse> GetAccessTokenAsync()
		{
			if (string.IsNullOrEmpty(_latestAccessToken?.RefreshToken))
			{
				_latestAccessToken = await RequestAccessTokenAsync();
			}
			else
			{
				_latestAccessToken = await RequestRefreshTokenAsync(_latestAccessToken.RefreshToken);
			}
			return _latestAccessToken;
		}

		public virtual async Task<TokenResponse> RequestAccessTokenAsync()
		{
			var response = await _httpClient.PostAsync(
				Endpoints.OAuthToken,
				new JsonContent(TokenRequest.For(_username, _password))
			);

			if (response.IsSuccessStatusCode)
			{
				return await response.Content.ReadAsJsonAsync<TokenResponse>();
			}
			var payload = await response.Content.ReadAsJsonAsync<AkeneoResponse>();
			throw new Exception($"Akeneo PIM replied with a non-successful status code {payload.Code}\n{payload.Message}");
		}

		public virtual async Task<TokenResponse> RequestRefreshTokenAsync(string refreshToken)
		{
			if (string.IsNullOrEmpty(refreshToken))
			{
				throw new Exception("No refresh token found. Try to create an access token.");
			}

			var response = await _httpClient.PostAsync(
				Endpoints.OAuthToken,
				new JsonContent(RefreshTokenRequest.For(refreshToken))
			);

			if (response.IsSuccessStatusCode)
			{
				return await response.Content.ReadAsJsonAsync<TokenResponse>();
			}
			var payload = await response.Content.ReadAsJsonAsync<AkeneoResponse>();
			throw new Exception($"Akeneo PIM replied with a non-successful status code {payload.Code}\n{payload.Message}");
		}

		private static string CreateAuthHeader(string clientId, string clientSecret)
		{
			return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes($"{clientId}:{clientSecret}"));
		}

		public void Dispose()
		{
			_httpClient?.Dispose();
		}
	}
}
