using System.Collections.Generic;
using System.Threading.Tasks;
using Akeneo.Consts;
using Akeneo.Model;

namespace Akeneo.Extensions
{
    public static class AkeneoClientExtensions
	{
		public static Task<List<TModel>> GetAllAsync<TModel>(this IAkeneoClient client, int limit = 10) where TModel : ModelBase
		{
			return client.GetAllAsync<TModel>(null, limit);
		}

		public static async Task<List<TModel>> GetAllAsync<TModel>(this IAkeneoClient client, string parentCode, int limit = 10) where TModel : ModelBase
		{
			var result = new List<TModel>();
			var page = 1;
			bool hasMore = false;
			do
			{
				var pagination = await client.GetManyAsync<TModel>(parentCode, page, limit);
				result.AddRange(pagination.GetItems());
				hasMore = pagination.Links.ContainsKey(PaginationLinks.Next);
				page++;
			} while (hasMore);
			return result;
		}
	}
}
