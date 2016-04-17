using SimpleEventSourcing.Messages;

namespace SimpleEventSourcing.Application
{
    public interface ICommandDispatcher
    {
        void Send<TCommand>(TCommand command) where TCommand : ICommand;
    }
}