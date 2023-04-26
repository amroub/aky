namespace aky.Foundation.Utility.HttpClient.Content
{
    using System.Net.Http;
    using System.Text;
    using aky.Foundation.Utility.Serialization;

    public class JsonContent : StringContent
    {
        public JsonContent(ISerializer serializer, object value)
            : base(serializer.Serialize(value), Encoding.UTF8, "application/json")
        {
        }

        public JsonContent(ISerializer serializer, object value, string mediaType)
            : base(serializer.Serialize(value), Encoding.UTF8, mediaType)
        {
        }
    }
}
