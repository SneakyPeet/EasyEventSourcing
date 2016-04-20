using System;
using EasyEventSourcing.EventSourcing.Domain;

namespace EasyEventSourcing.EventSourcing.Exceptions
{
    [Serializable]
    public class EventStreamNotFoundException : EventSourceException
    {
        public EventStreamNotFoundException(StreamIdentifier identifier)
            : base(string.Format("Stream Not Found Id: {0}", identifier.Value))
        {
        }
    }
}