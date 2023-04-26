namespace aky.Foundation.AzureServiceBus
{
    using System;
    using Microsoft.Azure.ServiceBus;

    public interface IServiceBusPersisterConnection : IDisposable
    {
        ServiceBusConnectionStringBuilder ServiceBusConnectionStringBuilder { get; }

        ITopicClient CreateTopicModel();

        ISubscriptionClient CreateSubscriptionModel(string subscriptionClientName);
    }
}
