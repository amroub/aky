namespace aky.Foundation.Ddd.Infrastructure
{
    using System;

    public class DomainEvent : IMessage
    {
        public int AggregateRootId { get; set; }

        public DateTime CreationTime { get; private set; } = DateTime.Now;
    }
}
