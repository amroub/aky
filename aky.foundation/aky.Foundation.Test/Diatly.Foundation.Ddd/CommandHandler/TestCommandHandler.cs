namespace Diatly.Foundation.Test.Diatly.Foundation.Ddd.CommandHandler
{
    using System;
    using System.Threading.Tasks;
    using global::Diatly.Foundation.Ddd.Handlers;
    using global::Diatly.Foundation.Test.Diatly.Foundation.Ddd.Commands;

    public class TestCommandHandler : IHandles<TestCommand>
    {
        public Task HandleAsync(TestCommand message)
        {
            return Task.CompletedTask;
        }
    }
}
