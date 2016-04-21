using EasyEventSourcing.Messages;

namespace EasyEventSourcing.Data
{
    public interface IEventObserver
    {
        void Notify(IEvent evt);
    }
}