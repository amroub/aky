using System.Diagnostics.CodeAnalysis;

namespace Akeneo.Common
{
    [ExcludeFromCodeCoverage]
    public class Endpoints
	{
		public const string OAuthToken = "api/oauth/v1/token";
		public const string Products = "api/rest/v1/products";
		public const string Categories = "api/rest/v1/categories";
		public const string Attributes = "api/rest/v1/attributes";
		public const string Families = "api/rest/v1/families";
		public const string MediaFiles = "api/rest/v1/media-files";
		public const string Locale = "api/rest/v1/locales";
        public const string Channel = "api/rest/v1/channels";
        public const string AttributeGroup = "api/rest/v1/attribute-groups";       
        public const string ProductModels = "api/rest/v1/product-models";
    }
}
