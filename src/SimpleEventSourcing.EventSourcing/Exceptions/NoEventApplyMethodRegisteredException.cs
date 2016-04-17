using System;
using SimpleEventSourcing.Messages;

namespace SimpleEventSourcing.EventSourcing.Exceptions
{
    [Serializable]
    public class NoEventApplyMethodRegisteredException : EventSourceException
    {
        public NoEventApplyMethodRegisteredException(IEvent evt, EventStreamItem eventStreamItem) 
            : base (string.Format("No Event Applier Registered For {0} on {1}", evt.GetType().Name, eventStreamItem.Name))
        {
        }
    }
}