using System.Collections.Generic;
using SimpleEventSourcing.Messages;

namespace SimpleEventSourcing.EventSourcing
{
    public interface IEventStore
    {
        IEnumerable<IEvent> GetByStreamId(StreamIdentifier streamId);
        void Save(List<EventStream> newEvents);
    }
}