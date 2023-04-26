namespace aky.Foundation.Infrastructure.Exceptions
{
    using System;

    public abstract class AggregateException : Exception
    {
        public AggregateException(Guid aggregateId, string message)
            : base(message)
        {
            this.AggregateId = aggregateId;
        }

        public Guid AggregateId { get; }

        public override string Message
        {
            get
            {
                return $"Aggregate Id : {this.AggregateId} - {base.Message}";
            }
        }
    }
}
