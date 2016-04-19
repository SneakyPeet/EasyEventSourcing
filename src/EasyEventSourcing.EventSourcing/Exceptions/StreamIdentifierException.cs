using System;

namespace EasyEventSourcing.EventSourcing.Exceptions
{
    [Serializable]
    public class StreamIdentifierException : EventSourceException
    {
        public StreamIdentifierException(string message) : base(message)
        {
        }
    }
}