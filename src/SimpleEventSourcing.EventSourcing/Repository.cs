using System;
using System.Collections.Generic;
using SimpleEventSourcing.Messages;

namespace SimpleEventSourcing.EventSourcing
{
    public class Repository : IRepository
    {
        private readonly IEventStore eventStore;
        public Repository(IEventStore eventStore)
        {
            this.eventStore = eventStore;
        }

        public T GetById<T>(Guid id) where T : EventStreamItem, new()
        {
            var streamItem = new T();
            var streamId = new StreamIdentifier(streamItem.Name, id);
            var history = this.eventStore.GetByStreamId(streamId);
            streamItem.LoadFromHistory(history);
            return streamItem;
        }

        public void Save(EventStreamItem streamItem)
        {
            this.Save(new List<EventStreamItem> { streamItem });
        }

        public void Save(IEnumerable<EventStreamItem> streamItems)
        {
            var newEvents = new List<EventStream>();
            foreach(var item in streamItems)
            {
                newEvents.Add(new EventStream(item.StreamIdentifier, item.GetUncommitedChanges()));
            }

            eventStore.Save(newEvents);

            foreach (var item in streamItems)
            {
                item.Commit();
            }
        }
    }
}