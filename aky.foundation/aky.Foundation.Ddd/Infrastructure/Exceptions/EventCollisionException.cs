namespace aky.Foundation.Infrastructure.Exceptions
{
    using System;

    public class EventCollisionException : AggregateException
    {
        private const string ERRORTEXT = "Data has been changed between loading and state changes.";

        public EventCollisionException(Guid aggregateId)
            : base(aggregateId, ERRORTEXT)
        {
        }

        public EventCollisionException(Guid aggregateId, string message)
            : base(aggregateId, message)
        {
        }
    }
}
