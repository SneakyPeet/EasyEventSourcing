using EasyEventSourcing.Messages;

namespace EasyEventSourcing.EventSourcing.Handlers
{
    public interface ICommandHandler<in TCommand> : IHandler where TCommand : ICommand
    {
        void Handle(TCommand cmd);
    }
}