using System;
using System.Collections.Generic;
using EasyEventSourcing.EventSourcing;
using EasyEventSourcing.EventSourcing.Domain;
using EasyEventSourcing.EventSourcing.Handlers;
using EasyEventSourcing.EventSourcing.Persistence;
using EasyEventSourcing.Messages;

namespace EasyEventSourcing.Data
{
    public class InMemoryEventStore : IEventStore
    {
        private readonly IEventDispatcher dispatcher;

        public InMemoryEventStore(IEventDispatcher dispatcher)
        {
            this.dispatcher = dispatcher;
        }

        public IEnumerable<IEvent> GetByStreamId(StreamIdentifier streamId)
        {
            throw new NotImplementedException();
        }

        public void Save(List<EventStoreStream> newEvents)
        {
            foreach(var eventStoreStream in newEvents)
            {
                foreach (var evt in eventStoreStream.Events)
                {
                    dispatcher.Send(evt);
                }
                
            }
            throw new NotImplementedException();
        }
    }
}
