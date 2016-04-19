using System.Collections.Generic;
using EasyEventSourcing.Messages;

namespace EasyEventSourcing.EventSourcing
{
    public interface IEventStore
    {
        IEnumerable<IEvent> GetByStreamId(StreamIdentifier streamId);
        void Save(List<EventStoreStream> newEvents);
    }
}