using EasyEventSourcing.EventSourcing.Handlers;
using EasyEventSourcing.Messages;

namespace EasyEventSourcing.Application.Write
{
    public class CommandDispatcher : ICommandDispatcher
    {
        private readonly ICommandHandlerFactory factory;

        public CommandDispatcher(ICommandHandlerFactory factory)
        {
            this.factory = factory;
        }
        public void Send<TCommand>(TCommand command) where TCommand : ICommand
        {
            var handler = this.factory.Resolve<TCommand>();
            handler.Handle(command);
        }
    }
}