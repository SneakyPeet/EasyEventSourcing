using SimpleEventSourcing.Messages;

namespace SimpleEventSourcing.EventSourcing
{
    public interface ICommandHandler<TCommand> where TCommand : ICommand
    {
        void Handle(TCommand command);
    }
}