using System;
using System.Collections.Generic;
using EasyEventSourcing.EventSourcing;
using EasyEventSourcing.Messages;

namespace EasyEventSourcing.Data
{
    public class InMemoryEventStore : IEventStore
    {
        public IEnumerable<IEvent> GetByStreamId(StreamIdentifier streamId)
        {
            throw new NotImplementedException();
        }

        public void Save(List<EventStoreStream> newEvents)
        {
            throw new NotImplementedException();
        }
    }
}
