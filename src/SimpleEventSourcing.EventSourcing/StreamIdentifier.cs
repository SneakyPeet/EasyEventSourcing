using SimpleEventSourcing.EventSourcing.Exceptions;
using System;

namespace SimpleEventSourcing.EventSourcing
{
    public class StreamIdentifier
    {
        public StreamIdentifier(string name, Guid id)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new StreamIdentifierException("StreamIdentifier Name Required");
            }
            if(id == null || id == Guid.Empty)
            {
                throw new StreamIdentifierException("StreamIdentifier Id Required");
            }
            this.Value = string.Format("{0}-{1}", name, id.ToString());
        }

        public string Value { get; private set; }
    }
}
