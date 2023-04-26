namespace aky.Foundation.AzureServiceBus
{
    using System;
    using Microsoft.Azure.ServiceBus;

    public class DefaultServiceBusPersisterConnection : IServiceBusPersisterConnection
    {
        private ITopicClient topicClient;

        private bool disposed;

        public DefaultServiceBusPersisterConnection(ServiceBusConnectionStringBuilder serviceBusConnectionStringBuilder)
        {
            this.ServiceBusConnectionStringBuilder = serviceBusConnectionStringBuilder ??
                throw new ArgumentNullException(nameof(serviceBusConnectionStringBuilder));
            this.topicClient = new TopicClient(this.ServiceBusConnectionStringBuilder, RetryPolicy.Default);
        }

        public ServiceBusConnectionStringBuilder ServiceBusConnectionStringBuilder { get; }

        public ISubscriptionClient CreateSubscriptionModel(string subscriptionClientName)
        {
            return new SubscriptionClient(this.ServiceBusConnectionStringBuilder, subscriptionClientName);
        }

        public ITopicClient CreateTopicModel()
        {
            if (this.topicClient.IsClosedOrClosing)
            {
                this.topicClient = new TopicClient(this.ServiceBusConnectionStringBuilder, RetryPolicy.Default);
            }

            return this.topicClient;
        }

        public void Dispose()
        {
            if (this.disposed)
            {
                return;
            }

            this.disposed = true;
        }
    }
}
