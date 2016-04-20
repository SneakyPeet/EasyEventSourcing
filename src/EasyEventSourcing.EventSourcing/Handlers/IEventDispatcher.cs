using EasyEventSourcing.Messages;

namespace EasyEventSourcing.EventSourcing.Handlers
{
    public interface IEventDispatcher
    {
        void Send(IEvent evt);
    }
}