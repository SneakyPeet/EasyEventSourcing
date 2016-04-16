using SimpleEventSourcing.Messages;

namespace SimpleEventSourcing.EventSourcing
{
    public interface IApplication
    {
        void Handle<TCommand>(TCommand command) where TCommand : ICommand;
    }
}