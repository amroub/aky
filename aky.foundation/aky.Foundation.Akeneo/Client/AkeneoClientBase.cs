using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Akeneo.Authentication;
using Akeneo.Http;
using Akeneo.Serialization;
using Newtonsoft.Json;

namespace Akeneo.Client
{
	public abstract class AkeneoClientBase : IDisposable
	{
		protected readonly IAuthenticator authenticator;
		protected readonly HttpClient httpClient;
		private const string bearerAuthHeader = "Bearer";

		protected AkeneoClientBase(Uri apiEndPoint, IAuthenticator authenticator)
		{
			this.authenticator = authenticator;
			httpClient = new HttpClient
			{
				BaseAddress = apiEndPoint,
				DefaultRequestHeaders = { Accept = { MediaTypeWithQualityHeaderValue.Parse("*/*") } }
			};
		}

		protected async Task<HttpResponseMessage> GetAsync(string url)
		{
			return await ExecuteAuthenticatedAsync((client, ctx) => client.GetAsync(ctx.RequestUrl),
				new HttpCallContext
				{
					RequestUrl = url
				});
		}

		protected async Task<HttpResponseMessage> DeleteAsync(string url)
		{
			return await ExecuteAuthenticatedAsync((client, ctx) => client.DeleteAsync(ctx.RequestUrl),
				new HttpCallContext
				{
					RequestUrl = url
				});
		}

		protected async Task<HttpResponseMessage> PatchAsJsonAsync<TContent>(string url, TContent content, JsonSerializerSettings jsonSettings = null)
		{
			return await ExecuteAuthenticatedAsync((client, ctx) => client.PatchAsJsonAsync(ctx.RequestUrl, ctx.Content, ctx.JsonSettings),
				new HttpCallContext
				{
					Content = content,
					RequestUrl = url,
					JsonSettings = jsonSettings ?? AkeneoSerializerSettings.Create
				});
		}

		protected async Task<HttpResponseMessage> PatchAsJsonCollectionAsync<TContent>(string url, IEnumerable<TContent> collection)
		{
			return await ExecuteAuthenticatedAsync((client, ctx) => client.PatchAsJsonCollectionAsync(ctx.RequestUrl, ctx.Content as IEnumerable<TContent>),
				new HttpCallContext
				{
					Content = collection,
					RequestUrl = url
				});
		}

		protected async Task<HttpResponseMessage> PostAsync<TContent>(string url, TContent content, JsonSerializerSettings jsonSettings = null)
		{
			return await ExecuteAuthenticatedAsync((client, ctx) => client.PostJsonAsync(ctx.RequestUrl, ctx.Content, ctx.JsonSettings),
				new HttpCallContext
				{
					Content = content,
					RequestUrl = url,
					JsonSettings = jsonSettings ?? AkeneoSerializerSettings.Create
				});
		}

		private async Task<HttpResponseMessage> ExecuteAuthenticatedAsync(Func<HttpClient, HttpCallContext, Task<HttpResponseMessage>> func, HttpCallContext context)
		{
			if (httpClient.DefaultRequestHeaders.Authorization == null)
			{
				await AddAuthHeaderAsync();
			}
			var response = await func(httpClient, context);
			if (response.StatusCode == HttpStatusCode.Unauthorized)
			{
				await AddAuthHeaderAsync();
				response = await func(httpClient, context);
			}
			
			return response;
		}

		protected async Task AddAuthHeaderAsync()
		{
			var tokenResponse = await authenticator.GetAccessTokenAsync();
			httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(bearerAuthHeader, tokenResponse.AccessToken);
		}

		public void Dispose()
		{
			httpClient?.Dispose();
			(authenticator as IDisposable)?.Dispose();
		}

		private class HttpCallContext
		{
			public string RequestUrl { get; set; }
			public object Content { get; set; }
			public CancellationToken CancellationToken { get; set; }
			public JsonSerializerSettings JsonSettings { get; set; }
		}
	}
}
