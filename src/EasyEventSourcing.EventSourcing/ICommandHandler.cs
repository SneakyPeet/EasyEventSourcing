using EasyEventSourcing.Messages;

namespace EasyEventSourcing.EventSourcing
{
    public interface ICommandHandler<in TCommand> : IHandler where TCommand : ICommand
    {
        void Handle(TCommand cmd);
    }

    public interface IHandler { }
}