namespace aky.Foundation.Ddd.Persistance
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using aky.Foundation.Ddd.Domain;
    using aky.Foundation.Ddd.Infrastructure;

    public interface IEventStore
    {
        Task SaveEventsAsync(AggregateRoot aggregate);

        IEnumerable<DomainEvent> GetEvents(int aggregateId);
    }
}
