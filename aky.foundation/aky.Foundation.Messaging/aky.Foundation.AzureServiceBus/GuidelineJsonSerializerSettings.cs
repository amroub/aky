namespace aky.Foundation.AzureServiceBus
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using Newtonsoft.Json.Serialization;

    public class GuidelineJsonSerializerSettings : JsonSerializerSettings
    {
        public GuidelineJsonSerializerSettings()
        {
            this.ContractResolver = new CamelCasePropertyNamesContractResolver();
            this.TypeNameHandling = TypeNameHandling.None;
            this.DateFormatHandling = DateFormatHandling.IsoDateFormat;
            this.NullValueHandling = NullValueHandling.Ignore;
            this.DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate;
            this.Converters.Add(new StringEnumConverter());
        }
    }
}
