namespace aky.Foundation.AzureServiceBus
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using aky.Foundation.AzureServiceBus.Specifications;
    using Microsoft.Azure.ServiceBus;

    public sealed class Subscriber : ISubscriber
    {
        private IServiceBusPersisterConnection serviceBusPersisterConnection;
        private List<SubscriptionClient> clientRegistry;

        public Subscriber(IServiceBusPersisterConnection serviceBusPersisterConnection)
        {
            this.serviceBusPersisterConnection = serviceBusPersisterConnection;
            this.clientRegistry = new List<SubscriptionClient>();
        }

        async Task ISubscriber.Subscribe<T>(string subscriptionName, Func<SubscriptionMessage<T>, Task> messageReceived)
        {
            await this.Subscribe<T>(subscriptionName, null, messageReceived);
        }

        async Task ISubscriber.Subscribe<T>(
            string subscriptionName,
            ISpecification filters,
            Func<SubscriptionMessage<T>, Task> messageReceived)
        {
            await this.Subscribe<T>(subscriptionName, filters, messageReceived);
        }

        private static RuleDescription CreateSqlFilter(string ruleName, ISpecification specification)
        {
            var sqlRule = new RuleDescription(ruleName)
            {
                Filter = new SqlFilter(specification.Result()),
            };

            return sqlRule;
        }

        private static void EnsureSubscriptionNameIsValid(string subscriptionFullName)
        {
            if (subscriptionFullName.Length > 50)
            {
                throw new Exception("The entity path/name of subscription '" + subscriptionFullName +
                                    "' exceeds the 50 character limit.");
            }
        }

        private static Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            // Console.WriteLine($"Message handler encountered an exception {exceptionReceivedEventArgs.Exception}.");
            // var context = exceptionReceivedEventArgs.ExceptionReceivedContext;
            // Console.WriteLine("Exception context for troubleshooting:");
            // Console.WriteLine($"- Endpoint: {context.Endpoint}");
            // Console.WriteLine($"- Entity Path: {context.EntityPath}");
            // Console.WriteLine($"- Executing Action: {context.Action}");
            return Task.CompletedTask;
        }

        private async Task Subscribe<T>(
            string subscriptionName,
            ISpecification filters,
            Func<SubscriptionMessage<T>, Task> messageReceived)
        {
            SubscriptionClient subscriptionClient = (SubscriptionClient)this.serviceBusPersisterConnection.CreateSubscriptionModel(subscriptionName);

            await this.CreateSubscription(
                subscriptionClient,
                subscriptionName,
                typeof(T),
                filters);

            this.clientRegistry.Add(subscriptionClient);

            MessageHandlerOptions options = new MessageHandlerOptions(ExceptionReceivedHandler)
            {
                AutoComplete = false,
                MaxConcurrentCalls = 1,
            };

            subscriptionClient.RegisterMessageHandler(
                async (message, token) =>
            {
                var subscriptionMessage = new SubscriptionMessage<T>(message);
                try
                {
                    await messageReceived(subscriptionMessage);

                    if (!subscriptionMessage.IsActioned)
                    {
                        await subscriptionClient.CompleteAsync(message.SystemProperties.LockToken);
                    }
                }
                catch
                {
                }
            }, options);
        }

        private async Task CreateSubscription(SubscriptionClient subscriptionClient, string subscriptionName, Type type, ISpecification customFilter = null)
        {
            string subscriptionFullName = subscriptionName;

            EnsureSubscriptionNameIsValid(subscriptionFullName);

            ISpecification messageTypeSpecification = new EqualSpecification(EventBusConstant.MessageType, type.Name);

            // Join the custom and base filter if necessary
            var filter = customFilter != null ? new AndSpecification(messageTypeSpecification, customFilter) : messageTypeSpecification;

            var defaultEventTypeRule = CreateSqlFilter(EventBusConstant.EventSubscriptionRule, filter);

            await subscriptionClient.GetRulesAsync()
                .ContinueWith(a =>
                {
                    if (!a.Result.Any(x => x.Name == EventBusConstant.EventSubscriptionRule))
                    {
                        subscriptionClient.AddRuleAsync(defaultEventTypeRule);
                    }
                });
        }
    }
}