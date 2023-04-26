namespace Akeneo.Client
{
    public class ValidationError
    {
        public string Property { get; set; }
        public string Message { get; set; }
        public string Attribute { get; set; }
        public string Locale { get; set; }
        public string Scope { get; set; }
        public string Currency { get; set; }
    }
}
