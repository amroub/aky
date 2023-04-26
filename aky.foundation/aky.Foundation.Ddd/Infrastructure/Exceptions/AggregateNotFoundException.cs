namespace aky.Foundation.Infrastructure.Exceptions
{
    using System;

    public class AggregateNotFoundException : AggregateException
    {
        private const string ERRORTEXT = "The aggregate you requested cannot be found.";

        public AggregateNotFoundException(Guid aggregateId)
            : base(aggregateId, ERRORTEXT)
        {
        }
    }
}
