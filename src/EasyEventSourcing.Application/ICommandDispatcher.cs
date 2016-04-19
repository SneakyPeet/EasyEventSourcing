using EasyEventSourcing.Messages;

namespace EasyEventSourcing.Application
{
    public interface ICommandDispatcher
    {
        void Send<TCommand>(TCommand command) where TCommand : ICommand;
    }
}