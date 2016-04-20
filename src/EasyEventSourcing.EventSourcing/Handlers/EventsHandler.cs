using System;
using System.Collections.Generic;
using EasyEventSourcing.EventSourcing.Exceptions;
using EasyEventSourcing.Messages;

namespace EasyEventSourcing.EventSourcing.Handlers
{
    public abstract class EventsHandler //because EventHandler is taken boo
    {
        private readonly Dictionary<Type, Action<IEvent>> handlers;

        protected EventsHandler()
        {
            this.handlers = new Dictionary<Type, Action<IEvent>>();
            this.RegisterHandlers();
        }

        protected abstract void RegisterHandlers();
        
        public void Handle(IEvent evt)
        {
            var evtType = evt.GetType();
            if (!this.handlers.ContainsKey(evtType))
            {
                throw new NoEventHandlerMethodRegisteredException(evt, this);
            }
            this.handlers[evtType](evt);
        }
        
        protected void RegisterHandler<TEvent>(Action<TEvent> handler) where TEvent : IEvent
        {
            this.handlers.Add(typeof(TEvent), x => handler((TEvent)x));
        }
    }
}