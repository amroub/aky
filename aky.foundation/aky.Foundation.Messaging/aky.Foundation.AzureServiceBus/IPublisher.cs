namespace aky.Foundation.AzureServiceBus
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IPublisher
    {
        Task SendBatch<T>(IEnumerable<T> messages)
            where T : class;

        Task Send<T>(T message)
            where T : class;
    }
}
