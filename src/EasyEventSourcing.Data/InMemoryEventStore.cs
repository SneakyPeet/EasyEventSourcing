using System.Collections.Generic;
using EasyEventSourcing.EventSourcing.Domain;
using EasyEventSourcing.EventSourcing.Exceptions;
using EasyEventSourcing.EventSourcing.Handlers;
using EasyEventSourcing.EventSourcing.Persistence;
using EasyEventSourcing.Messages;

namespace EasyEventSourcing.Data
{
    public class InMemoryEventStore : IEventStore
    {
        private readonly IEventDispatcher dispatcher;

        private readonly Dictionary<string, List<IEvent>> store = new Dictionary<string, List<IEvent>>();

        public InMemoryEventStore(IEventDispatcher dispatcher)
        {
            this.dispatcher = dispatcher;
        }

        public IEnumerable<IEvent> GetByStreamId(StreamIdentifier streamId)
        {
            if (store.ContainsKey(streamId.Value))
            {
                return store[streamId.Value].AsReadOnly();
            }
            throw new EventStreamNotFoundException(streamId);
        }

        public void Save(List<EventStoreStream> newEvents)
        {
            foreach (var eventStoreStream in newEvents)
            {
                this.PersistEvents(eventStoreStream);
                this.DispatchEvents(eventStoreStream.Events);
            }
        }

        private void PersistEvents(EventStoreStream eventStoreStream)
        {
            if(store.ContainsKey(eventStoreStream.Id))
            {
                store[eventStoreStream.Id].AddRange(eventStoreStream.Events);
            }
            else
            {
                store.Add(eventStoreStream.Id, eventStoreStream.Events);
            }
        }

        private void DispatchEvents(IEnumerable<IEvent> newEvents)
        {
            foreach (var evt in newEvents)
            {
                this.dispatcher.Send(evt); //todo fix
            }
        }
    }
}
