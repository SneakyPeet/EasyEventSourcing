using System;
using System.Runtime.Serialization;

namespace SimpleEventSourcing.EventSourcing.Exceptions
{
    [Serializable]
    public class StreamIdentifierException : EventSourceException
    {
        public StreamIdentifierException(string message) : base(message)
        {
        }
    }
}