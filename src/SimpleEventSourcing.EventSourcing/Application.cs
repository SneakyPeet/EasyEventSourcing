using SimpleEventSourcing.Messages;

namespace SimpleEventSourcing.EventSourcing
{
    public class Application : IApplication
    {
        private readonly ICommandHandlerFactory factory;

        public Application(ICommandHandlerFactory factory)
        {
            this.factory = factory;
        }
        public void Handle<TCommand>(TCommand command) where TCommand : ICommand
        {
            var handler = factory.Resolve<TCommand>();
            handler.Handle(command);
        }
    }
}