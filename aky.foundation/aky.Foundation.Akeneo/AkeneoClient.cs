using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Akeneo.Authentication;
using Akeneo.Client;
using Akeneo.Common;
using Akeneo.Consts;
using Akeneo.Exceptions;
using Akeneo.Http;
using Akeneo.Model;
using Akeneo.Model.Attributes;
using Akeneo.Search;
using Akeneo.Serialization;

namespace Akeneo
{
    public class AkeneoClient : AkeneoClientBase, IAkeneoClient
    {
        private readonly EndpointResolver _endpointResolver;
        private readonly SearchQueryBuilder _searchBuilder;

        public AkeneoClient(AkeneoOptions options)
            : this(options.ApiEndpoint, new Authenticator(options.ApiEndpoint, options.ClientId, options.ClientSecret, options.UserName, options.Password)) { }

        public AkeneoClient(Uri apiEndPoint, IAuthenticator authClient) : base(apiEndPoint, authClient)
        {
            _endpointResolver = new EndpointResolver();
            _searchBuilder = new SearchQueryBuilder();
        }

        public async Task<TModel> GetAsync<TModel>(string code) where TModel : ModelBase
        {
            var endpoint = _endpointResolver.ForResource<TModel>(code);
            var response = await GetAsync(endpoint);
            return response.IsSuccessStatusCode
                ? await response.Content.ReadAsJsonAsync<TModel>()
                : default(TModel);
        }

        public async Task<TModel> GetAsync<TModel>(string parentCode, string code) where TModel : ModelBase
        {
            var endpoint = _endpointResolver.ForResource<TModel>(parentCode, code);
            var response = await GetAsync(endpoint);
            return response.IsSuccessStatusCode
                ? await response.Content.ReadAsJsonAsync<TModel>()
                : default(TModel);
        }

        public Task<PaginationResult<TModel>> SearchAsync<TModel>(IEnumerable<Criteria> criterias) where TModel : ModelBase
        {
            var queryString = _searchBuilder.GetQueryString(criterias);
            return FilterAsync<TModel>(queryString);
        }

        public async Task<PaginationResult<TModel>> FilterAsync<TModel>(string queryString) where TModel : ModelBase
        {
            var endpoint = _endpointResolver.ForResourceType<TModel>();
            var response = await GetAsync($"{endpoint}{queryString}");
            var result = await response.Content.ReadAsJsonAsync<PaginationResult<TModel>>();
            result.Code = response.StatusCode;
            return result;
        }

        public Task<PaginationResult<TModel>> GetManyAsync<TModel>(int page = 1, int limit = 10, bool withCount = false) where TModel : ModelBase
        {
            return GetManyAsync<TModel>(null, page, limit, withCount);
        }

        public async Task<PaginationResult<TModel>> GetManyAsync<TModel>(string parentCode, int page = 1, int limit = 10, bool withCount = false) where TModel : ModelBase
        {
            var endpoint = _endpointResolver.ForPagination<TModel>(parentCode, page, limit, withCount);
            var response = await GetAsync(endpoint);
            return response.IsSuccessStatusCode
                ? await response.Content.ReadAsJsonAsync<PaginationResult<TModel>>()
                : PaginationResult<TModel>.Empty;
        }

        public async Task<AkeneoResponse> CreateAsync<TModel>(TModel model) where TModel : ModelBase
        {
            var option = model as AttributeOption;
            var variant = model as FamilyVariantWithParent;
            var endpoint = _endpointResolver.ForResourceType<TModel>(option?.Attribute ?? variant?.ParentCode ?? string.Empty);
            HttpResponseMessage response;
            if (variant!=null)
            {
                response = await PostAsync(endpoint, variant?.ToFamilyVariant(), AkeneoSerializerSettings.Create);
            }
            else
            {
                response = await PostAsync(endpoint, model, AkeneoSerializerSettings.Create);
            }

            return response.IsSuccessStatusCode
                ? AkeneoResponse.Success(response.StatusCode, new KeyValuePair<string, PaginationLink>(PaginationLinks.Location, new PaginationLink { Href = response.Headers?.Location?.ToString() }))
                : await response.Content.ReadAsJsonAsync<AkeneoResponse>();
        }

        public async Task<AkeneoResponse> UpdateAsync<TModel>(TModel model) where TModel : ModelBase
        {
            var endpoint = _endpointResolver.ForResource(model);
            var response = await PatchAsJsonAsync(endpoint, model, AkeneoSerializerSettings.Update);
            return response.IsSuccessStatusCode
                ? AkeneoResponse.Success(response.StatusCode, new KeyValuePair<string, PaginationLink>(PaginationLinks.Location, new PaginationLink { Href = response.Headers?.Location?.ToString() }))
                : await response.Content.ReadAsJsonAsync<AkeneoResponse>();
        }

        public async Task<AkeneoResponse> UpdateAsync<TModel>(string identifier, object model) where TModel : ModelBase
        {
            var endpoint = $"{_endpointResolver.ForResourceType<TModel>()}/{identifier}";
            var response = await PatchAsJsonAsync(endpoint, model, AkeneoSerializerSettings.Update);
            return response.IsSuccessStatusCode
                ? AkeneoResponse.Success(response.StatusCode, new KeyValuePair<string, PaginationLink>(PaginationLinks.Location, new PaginationLink { Href = response.Headers?.Location?.ToString() }))
                : await response.Content.ReadAsJsonAsync<AkeneoResponse>();
        }

        public async Task<List<AkeneoBatchResponse>> CreateOrUpdateAsync<TModel>(IEnumerable<TModel> model) where TModel : ModelBase
        {
            var option = model.FirstOrDefault() as AttributeOption;
            var variant = model.FirstOrDefault() as FamilyVariantWithParent;
            var endpoint = _endpointResolver.ForResourceType<TModel>(option?.Attribute??variant?.ParentCode);
            HttpResponseMessage response;
            if (variant != null)
            {
                response = await PatchAsJsonCollectionAsync(endpoint, model.Cast<FamilyVariantWithParent>().Select(fv=>fv.ToFamilyVariant()).ToList());
            }
            else
            {
                response = await PatchAsJsonCollectionAsync(endpoint, model);
            }

            var contentStr = await response.Content.ReadAsStringAsync();
            contentStr += string.IsNullOrEmpty(response.ReasonPhrase)?"":$" - {response.ReasonPhrase}" ;
            return AkeneoCollectionSerializer.Deserialize<AkeneoBatchResponse>(contentStr).ToList();
        }

        public async Task<AkeneoResponse> DeleteAsync<TModel>(string code) where TModel : ModelBase
        {
            if (!typeof(TModel).GetTypeInfo().IsAssignableFrom(typeof(Product)))
                throw new NotSupportedActionException("Delete API action is only supported for Product.");

            var endpoint = _endpointResolver.ForResource<TModel>(code);
            var response = await DeleteAsync(endpoint);
            return response.IsSuccessStatusCode
                ? AkeneoResponse.Success(response.StatusCode)
                : await response.Content.ReadAsJsonAsync<AkeneoResponse>();
        }

        public async Task<AkeneoResponse> UploadAsync(MediaUpload media)
        {
            if (!File.Exists(media.FilePath))
            {
                throw new FileNotFoundException($"File with path {media.FilePath} not found.");
            }
            var filename = media.FileName ?? Path.GetFileName(media.FilePath);
            var formContent = new MultipartFormDataContent
            {
                {new JsonContent(media.Product, AkeneoSerializerSettings.Update) , "product" },
                { new StreamContent(File.OpenRead(media.FilePath)), "file", filename }
            };

            var response = await httpClient.PostAsync(Endpoints.MediaFiles, formContent);
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                await AddAuthHeaderAsync();
                response = await httpClient.PostAsync(Endpoints.MediaFiles, new MultipartFormDataContent
                {
                    {new JsonContent(media.Product, AkeneoSerializerSettings.Update) , "product" },
                    { new StreamContent(File.OpenRead(media.FilePath)), "file", filename }
                });
            }
            return response.IsSuccessStatusCode
                ? AkeneoResponse.Success(response.StatusCode, new KeyValuePair<string, PaginationLink>(PaginationLinks.Location, new PaginationLink { Href = response.Headers?.Location?.ToString() }))
                : await response.Content.ReadAsJsonAsync<AkeneoResponse>();
        }

        public async Task<MediaDownload> DownloadAsync(string mediaCode)
        {
            var response = await GetAsync($"{Endpoints.MediaFiles}/{mediaCode}/download");
            if (response.StatusCode != HttpStatusCode.OK)
            {
                var body = await response.Content.ReadAsJsonAsync<AkeneoResponse>();
                throw new OperationUnsuccessfulException($"Unable to load Media File '{mediaCode}'.", body);
            }
            return new MediaDownload
            {
                FileName = response.Content.Headers.ContentDisposition.FileName,
                Stream = await response.Content.ReadAsStreamAsync()
            };
        }
    }
}
