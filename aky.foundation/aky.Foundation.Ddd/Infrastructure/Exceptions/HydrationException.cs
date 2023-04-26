namespace aky.Foundation.Infrastructure.Exceptions
{
    using System;

    public class HydrationException : AggregateException
    {
        private const string ERRORTEXT = "Loading the data failed.";

        public HydrationException(Guid aggregateId)
            : base(aggregateId, ERRORTEXT) { }
    }
}
