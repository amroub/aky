namespace aky.Foundation.Utility.Serialization
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using Newtonsoft.Json.Serialization;

    public sealed class JsonSerializer : ISerializer
    {
        private readonly JsonSerializerSettings settings;

        public JsonSerializer()
        {
            this.settings = new Newtonsoft.Json.JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                MissingMemberHandling = MissingMemberHandling.Ignore,
                NullValueHandling = NullValueHandling.Include,
                DefaultValueHandling = DefaultValueHandling.Include,
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                DateTimeZoneHandling = DateTimeZoneHandling.Utc,
            };

            this.settings.Converters.Add(new StringEnumConverter());
        }

        string ISerializer.Serialize<T>(T value)
        {
            return JsonConvert.SerializeObject(value, this.settings);
        }

        T ISerializer.Deserialize<T>(string value)
        {
            return JsonConvert.DeserializeObject<T>(value, this.settings);
        }
    }
}
