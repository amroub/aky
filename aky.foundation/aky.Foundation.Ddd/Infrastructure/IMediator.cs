namespace aky.Foundation.Ddd.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using aky.Foundation.Ddd.Commands;

    public interface IMediator
    {
        Task InvokeEventHandlerAsync<T>(T @event)
            where T : class;
        Task InvokeEventHandlerAsync(object @event);
        
        Task SendCommandAsync<T>(T command)
            where T : class, ICommand;
    }
}
