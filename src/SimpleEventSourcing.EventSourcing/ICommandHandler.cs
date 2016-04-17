using SimpleEventSourcing.Messages;

namespace SimpleEventSourcing.EventSourcing
{
    public interface ICommandHandler<TCommand> : IHandler where TCommand : ICommand
    {
        void Handle(TCommand message);
    }

    public interface IHandler { }
}