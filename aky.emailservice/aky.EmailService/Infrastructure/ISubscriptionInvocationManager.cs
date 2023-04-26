namespace aky.EmailService.Infrastructure
{
    using aky.Foundation.AzureServiceBus;
    using System.Threading.Tasks;

    public interface ISubscriptionInvocationManager
    {
        Task HandleEvents<T>(SubscriptionMessage<T> subscriptionMessage)
            where T : class;
    }
}
