﻿using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Akeneo.Serialization
{
	public class AkeneoSerializerSettings
	{
		public static readonly IContractResolver AkeneoContractResolver = new CamelCasePropertyNamesContractResolver
		{
			NamingStrategy = new SnakeCaseNamingStrategy()
		};

		public static readonly JsonSerializerSettings Create = new JsonSerializerSettings
		{
			Converters = { new AttributeBaseConverter(), new ProductConverter() },
			ContractResolver = AkeneoContractResolver,
			NullValueHandling = NullValueHandling.Ignore
		};

		public static readonly JsonSerializerSettings Update = new JsonSerializerSettings
		{
			Converters = { new AttributeBaseConverter() },
			ContractResolver = AkeneoContractResolver
		};
	}
}
