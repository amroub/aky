namespace Diatly.Foundation.Test.Diatly.Foundation.Ddd
{
    using System;
    using System.Threading.Tasks;
    using global::Diatly.Foundation.Ddd.Infrastructure;
    using global::Diatly.Foundation.Test.Diatly.Foundation.Ddd.Commands;
    using Xunit;

    public class MediatorTest : BaseFixture
    {
        private IMediator mediator;

        public MediatorTest()
        {
            this.mediator = this.Mediator;
        }

        [Fact]
        public async Task SendCommandAsync_SendCommandToHandler_Pass()
        {
            var command = new TestCommand();

            await this.mediator.SendCommandAsync(command);

            Assert.True(true);
        }

        [Fact]
        public async Task SendCommandAsync_SendCommandToHandlerButNoHandlerIsThere_Pass()
        {
            var command = new OrderCreatedCommand();

            await this.mediator.SendCommandAsync(command);

            Assert.True(true);
        }
    }
}
