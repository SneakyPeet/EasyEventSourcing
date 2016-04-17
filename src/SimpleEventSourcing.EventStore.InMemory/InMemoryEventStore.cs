using System;
using System.Collections.Generic;
using SimpleEventSourcing.EventSourcing;
using SimpleEventSourcing.Messages;

namespace SimpleEventSourcing.EventStore.InMemory
{
    public class InMemoryEventStore : IEventStore
    {
        public IEnumerable<IEvent> GetByStreamId(string streamId)
        {
            throw new NotImplementedException();
        }

        public void Save(List<IEvent> newEvents)
        {
            throw new NotImplementedException();
        }
    }
}
