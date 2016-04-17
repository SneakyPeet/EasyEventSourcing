using System;
using System.Runtime.Serialization;

namespace SimpleEventSourcing.EventSourcing
{
    [Serializable]
    public class EventSourceException : Exception
    {
        public EventSourceException()
        {
        }

        public EventSourceException(string message) : base(message)
        {
        }

        public EventSourceException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected EventSourceException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}