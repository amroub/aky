namespace aky.Foundation.AzureServiceBus
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.Azure.ServiceBus;
    using Newtonsoft.Json;

    public class Publisher : IPublisher
    {
        private IServiceBusPersisterConnection serviceBusPersisterConnection;

        public Publisher(IServiceBusPersisterConnection serviceBusPersisterConnection)
        {
            this.serviceBusPersisterConnection = serviceBusPersisterConnection;
        }

        public async Task Send<T>(T @event)
            where T : class
        {
            var eventName = @event.GetType().Name;
            var jsonMessage = JsonConvert.SerializeObject(@event);
            var body = Encoding.UTF8.GetBytes(jsonMessage);

            var message = new Message
            {
                MessageId = Guid.NewGuid().ToString(),
                Body = body,
                Label = eventName,
            };

            AppendCustomProperties(eventName, message);

            var topicClient = this.serviceBusPersisterConnection.CreateTopicModel();

            await topicClient.SendAsync(message);
        }

        public async Task SendBatch<T>(IEnumerable<T> events)
            where T : class
        {
            var tasks = new List<Task>();

            foreach (var @event in events)
            {
                tasks.Add(this.Send(@event));
            }

            await Task.WhenAll(tasks);
        }

        private static void AppendCustomProperties(
            string messageType,
            Message message)
        {
            message.UserProperties.Add(EventBusConstant.MessageType, messageType);
        }
    }
}
