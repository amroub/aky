namespace aky.Foundation.AzureServiceBus
{
    using System;
    using System.Threading.Tasks;
    using aky.Foundation.AzureServiceBus.Specifications;

    public interface ISubscriber
    {
        Task Subscribe<T>(string subscriptionName, Func<SubscriptionMessage<T>, Task> messageReceived)
            where T : class;

        Task Subscribe<T>(
            string subscriptionName,
            ISpecification filters,
            Func<SubscriptionMessage<T>, Task> messageReceived)
            where T : class;
    }
}
