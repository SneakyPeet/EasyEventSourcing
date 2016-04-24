using EasyEventSourcing.Messages;

namespace EasyEventSourcing.Data
{
    public interface IEventObserver
    {
        void Notify<TEvent>(TEvent evt) where TEvent : IEvent;
    }
}