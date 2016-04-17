using System.Collections.Generic;
using SimpleEventSourcing.Messages;

namespace SimpleEventSourcing.EventSourcing
{
    public interface IEventStore
    {
        IEnumerable<IEvent> GetByStreamId(string streamId);
        void Save(List<IEvent> newEvents);
    }
}