using System.Collections.Generic;
using System.Linq;
using EasyEventSourcing.EventSourcing.Domain;
using EasyEventSourcing.Messages;

namespace EasyEventSourcing.EventSourcing.Persistence
{
    public class EventStoreStream
    {
        public EventStoreStream(StreamIdentifier identifier, IEnumerable<IEvent> events)
        {
            this.Id = identifier.Value;
            this.Events = events.ToList();
        }

        public List<IEvent> Events { get; private set; }
        public string Id { get; private set; }
    }
}
