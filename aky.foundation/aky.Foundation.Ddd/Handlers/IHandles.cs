namespace aky.Foundation.Ddd.Handlers
{
    using System.Threading.Tasks;
    using aky.Foundation.Ddd.Infrastructure;

    public interface IHandles<T>
        where T : IMessage
    {
        Task HandleAsync(T message);
    }
}
