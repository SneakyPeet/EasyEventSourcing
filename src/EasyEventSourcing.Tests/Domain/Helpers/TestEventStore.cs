using System;
using System.Collections.Generic;
using System.Linq;
using EasyEventSourcing.EventSourcing;
using EasyEventSourcing.Messages;

namespace EasyEventSourcing.Tests.Domain.Helpers
{
    class TestEventStore : IEventStore 
    {
        private Dictionary<string, List<EventStreamEvent>> eventStreams = new Dictionary<string, List<EventStreamEvent>>();

        public IEnumerable<IEvent> GetByStreamId(StreamIdentifier streamId)
        {
            if(this.eventStreams.ContainsKey(streamId.Value))
            {
                return this.eventStreams[streamId.Value].ToEvents();
            }
            throw new Exception("Stream not found");
        }

        public void Save(List<EventStoreStream> newEvents)
        {
            foreach (var stream in newEvents)
            {
                var events = stream.Events.ToEventStreamEvents();
                if (this.eventStreams.ContainsKey(stream.Id))
                {
                    this.eventStreams[stream.Id].AddRange(events);
                }
                else
                {
                    this.eventStreams.Add(stream.Id, events);
                }
            }
            
        }

        internal void CommitInitialEvents()
        {
            var events = this.eventStreams.SelectMany(x => x.Value);
            foreach (var evt in events)
            {
                evt.Commit();
            }
        }

        internal List<IEvent> NewEvents()
        {
            return this.eventStreams.SelectMany(x => x.Value)
                .Where(x => !x.IsCommitted)
                .Select(x => x.Event)
                .ToList();
        }

        internal void Complete()
        {
            this.eventStreams.Clear();
        }
    }
}
