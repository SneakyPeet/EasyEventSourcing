using System;
using EasyEventSourcing.Data;
using EasyEventSourcing.EventSourcing.Handlers;
using EasyEventSourcing.Messages;

namespace EasyEventSourcing.EventProcessing
{
    public class EventProcessor : IEventObserver
    {
        private readonly IEventDispatcher dispatcher;
        private Action unsubscribe;

        public EventProcessor(InMemoryEventStore store, IEventDispatcher dispatcher)
        {
            this.dispatcher = dispatcher;
            this.unsubscribe = store.Subscribe(this);
        }

        public void Notify<TEvent>(TEvent evt) where TEvent : IEvent
        {
            dispatcher.Send(evt);
        }
    }
}
