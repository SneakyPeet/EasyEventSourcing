using EasyEventSourcing.Messages;

namespace EasyEventSourcing.EventSourcing.Handlers
{
    public interface IEventHandler<in TEvent> : IHandler where TEvent : IEvent
    {
        void Handle(TEvent evt);
    }
}