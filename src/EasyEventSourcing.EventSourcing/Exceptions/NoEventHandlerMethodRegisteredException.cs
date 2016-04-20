using System;
using EasyEventSourcing.EventSourcing.Handlers;
using EasyEventSourcing.Messages;

namespace EasyEventSourcing.EventSourcing.Exceptions
{
    [Serializable]
    public class NoEventHandlerMethodRegisteredException : EventSourceException
    {
        public NoEventHandlerMethodRegisteredException(IEvent evt, EventsHandler hander)
            : base(string.Format("No EventsHandler Registered For {0} on {1}", evt.GetType().Name, hander.GetType().Name))
        {
        }
    }
}