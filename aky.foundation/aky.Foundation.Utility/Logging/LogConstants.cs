namespace aky.Foundation.Utility.Logging
{
    public static class LogConstants
    {
        public const string Template = "{EntityType}, {EventId}, {Status}, {CorrelationId}, {Source}, {Description}";

        public enum Entity
        {
            Product,
            Family,
            Attribute,
            ProductFeed
        }

        public enum Source
        {
            Publisher,
            Subscriber
        }

        public enum Status
        {
            Succeeded,
            Failed,
            Discarded
        }
    }
}
