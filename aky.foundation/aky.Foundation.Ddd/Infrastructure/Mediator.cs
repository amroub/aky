namespace aky.Foundation.Ddd.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.ExceptionServices;
    using System.Threading.Tasks;
    using CommonServiceLocator;
    using aky.Foundation.Ddd.Handlers;
    using aky.Foundation.Ddd.Infrastructure.Exceptions;
    using aky.Foundation.Ddd.Persistance;

    public sealed class Mediator : IMediator
    {
        public Mediator()
        {
        }

        Task IMediator.SendCommandAsync<T>(T command)
        {
            return this.SendCommandAsync<T>(command);
        }

        private async Task SendCommandAsync<T>(T command)
            where T : class
        {
            if (command == null)
            {
                return;
            }

            if (!ServiceLocator.IsLocationProviderSet)
            {
                throw new ServiceLocatorException();
            }

            // Get a instance of the generic handler's type
            Type genericType = typeof(IHandles<>);

            // Get and instance of the messages type
            Type objectType = genericType.MakeGenericType(command.GetType());

            // Get a list of all the handlers for this message type
            // var handlers = TypeLocator.ResolveMany(objectType);
            var handlers = ServiceLocator.Current.GetAllInstances(objectType);

            if (handlers != null && handlers.Any())
            {
                if (handlers.Count() > 1)
                {
                    Trace.WriteLine("A command should only have one handler.");
                    return;
                }

                var handler = handlers.First();

                try
                {
                    await (Task)objectType.InvokeMember("HandleAsync", BindingFlags.InvokeMethod, null, handler, new[] { command });
                }
                catch (TargetInvocationException x)
                {
                    if (x.InnerException != null)
                    {
                        ExceptionDispatchInfo.Capture(x.InnerException).Throw();
                    }

                    throw;
                }
            }
            else
            {
                Trace.WriteLine(string.Format("No handler for the following command: {0}", command.GetType().FullName));
            }
        }
        async Task IMediator.InvokeEventHandlerAsync(object @event)
        {
            await InvokeEventHadlerAsync(@event);
        }
        async Task IMediator.InvokeEventHandlerAsync<T>(T @event)
        {
            await InvokeEventHadlerAsync(@event);
        }
        private async Task InvokeEventHadlerAsync(object @event)
        {
            if (@event == null)
            {
                return;
            }

            if (!ServiceLocator.IsLocationProviderSet)
            {
                throw new ServiceLocatorException();
            }

            // Get a instance of the generic handler's type
            Type genericType = typeof(IHandles<>);

            // Get and instance of the domain events type
            Type objectType = genericType.MakeGenericType(@event.GetType());

            // Get a list of all the handlers for this message type
            //var handlers = TypeLocator.ResolveMany(objectType);
            var handlers = ServiceLocator.Current.GetAllInstances(objectType);

            if (handlers != null && handlers.Any())
            {
                var tasks = new List<Task>();

                foreach (var handler in handlers)
                {
                    tasks.Add(Task.Run(async () =>
                    {
                        try
                        {
                            await (Task)objectType.InvokeMember("HandleAsync", BindingFlags.InvokeMethod, null, handler, new[] { @event });
                        }
                        catch
                        {
                            // push the message to an error queue identifying which handler failed
                            Trace.WriteLine(string.Format("Exception handling {0}", @event.GetType().FullName));
                        }
                    }));
                }

                await Task.WhenAll(tasks);
            }
        }
    }
}
