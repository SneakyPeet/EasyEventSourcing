using System;
using System.Collections.Generic;
using System.Linq;
using SimpleEventSourcing.EventSourcing;
using SimpleEventSourcing.Messages;

namespace SimpleEventSourcing.Tests.Domain.Helpers
{
    class TestEventStore : IEventStore 
    {
        private Dictionary<string, List<EventStreamEvent>> eventStreams = new Dictionary<string, List<EventStreamEvent>>();

        public IEnumerable<IEvent> GetByStreamId(StreamIdentifier streamId)
        {
            if(eventStreams.ContainsKey(streamId.Value))
            {
                return eventStreams[streamId.Value].ToEvents();
            }
            throw new Exception("Stream not found");
        }

        public void Save(List<EventStream> newEvents)
        {
            foreach (var stream in newEvents)
            {
                var events = stream.Events.ToEventStreamEvents();
                if (eventStreams.ContainsKey(stream.Id))
                {
                    eventStreams[stream.Id].AddRange(events);
                }
                else
                {
                    eventStreams.Add(stream.Id, events);
                }
            }
            
        }

        internal void CommitInitialEvents()
        {
            var events = eventStreams.SelectMany(x => x.Value);
            foreach (var evt in events)
            {
                evt.Commit();
            }
        }

        internal List<IEvent> NewEvents()
        {
            return eventStreams.SelectMany(x => x.Value)
                .Where(x => !x.IsCommitted)
                .Select(x => x.Event)
                .ToList();
        }

        internal void Complete()
        {
            eventStreams.Clear();
        }
    }
}
