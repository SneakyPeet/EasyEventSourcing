using System.Collections.Generic;
using EasyEventSourcing.Messages;

namespace EasyEventSourcing.EventSourcing.Handlers
{
    public interface IEventDispatcher
    {
        void Send<TEvent>(TEvent evt) where TEvent : IEvent;
    }
}