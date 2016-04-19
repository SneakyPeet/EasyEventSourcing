using System;
using System.Collections.Generic;

namespace EasyEventSourcing.EventSourcing
{
    public class Repository : IRepository
    {
        private readonly IEventStore eventStore;
        public Repository(IEventStore eventStore)
        {
            this.eventStore = eventStore;
        }

        public T GetById<T>(Guid id) where T : EventStream, new()
        {
            var streamItem = new T();
            var streamId = new StreamIdentifier(streamItem.Name, id);
            var history = this.eventStore.GetByStreamId(streamId);
            streamItem.LoadFromHistory(history);
            return streamItem;
        }

        public void Save(params EventStream[] streamItems)
        {
            var newEvents = new List<EventStoreStream>();
            foreach(var item in streamItems)
            {
                newEvents.Add(new EventStoreStream(item.StreamIdentifier, item.GetUncommitedChanges()));
            }

            this.eventStore.Save(newEvents);

            foreach (var item in streamItems)
            {
                item.Commit();
            }
        }
    }
}