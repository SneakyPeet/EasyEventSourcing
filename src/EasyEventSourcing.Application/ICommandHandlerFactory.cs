using EasyEventSourcing.EventSourcing;
using EasyEventSourcing.Messages;

namespace EasyEventSourcing.Application
{
    public interface ICommandHandlerFactory
    {
        ICommandHandler<TCommand> Resolve<TCommand>() where TCommand : ICommand;
    }
}