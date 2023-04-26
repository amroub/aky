namespace aky.EmailService.Infrastructure
{
    using System;
    using System.Threading.Tasks;
    using aky.Foundation.AzureServiceBus;
    using aky.Foundation.Ddd.Infrastructure;
    using Microsoft.Extensions.Logging;

    public class SubscriptionInvocationManager : ISubscriptionInvocationManager
    {
        private readonly IMediator mediator;
        private readonly ILogger<SubscriptionInvocationManager> logger;

        public SubscriptionInvocationManager(
            IMediator mediator,
            ILogger<SubscriptionInvocationManager> logger)
        {
            this.mediator = mediator;
            this.logger = logger;
        }

        public async Task HandleEvents<T>(SubscriptionMessage<T> subscriptionMessage)
            where T : class
        {
            var @event = subscriptionMessage.GetBody();

            try
            {
                await this.mediator.InvokeEventHandlerAsync(@event);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, $"Subscription has error while locating event handler for event of: {nameof(T)}");
            }
        }
    }
}
