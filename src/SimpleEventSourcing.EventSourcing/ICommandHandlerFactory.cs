using SimpleEventSourcing.Messages;

namespace SimpleEventSourcing.EventSourcing
{
    public interface ICommandHandlerFactory
    {
        ICommandHandler<TCommand> Resolve<TCommand>() where TCommand : ICommand;
    }
}