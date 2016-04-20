using System.Collections.Generic;
using EasyEventSourcing.EventSourcing.Handlers;
using EasyEventSourcing.Messages;

namespace EasyEventSourcing.Application.Read
{
    public class EventDispatcher : IEventDispatcher
    {
        private readonly IEventHandlerFactory factory;

        public EventDispatcher(IEventHandlerFactory factory)
        {
            this.factory = factory;
        }
        public void Send<TEvent>(TEvent evt) where TEvent : IEvent
        {
            var handlers = this.factory.Resolve<TEvent>();
            foreach (var eventHandler in handlers)
            {
                eventHandler.Handle(evt);
            }
        }
    }
}