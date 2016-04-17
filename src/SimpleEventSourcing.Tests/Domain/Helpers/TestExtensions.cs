using System.Collections.Generic;
using SimpleEventSourcing.Messages;
using System.Linq;

namespace SimpleEventSourcing.Tests.Domain.Helpers
{
    public static class TestExtensions
    {
        public static List<T> And<T>(this List<T> history, T message)
        {
            history.Add(message);
            return history;
        }

        
    }

    public static class Expected
    {
        public static List<IEvent> Event(IEvent evt)
        {
            return new List<IEvent> { evt };
        }
    }

    public static class StoreExtensions
    {
        public static IEnumerable<IEvent> ToEvents(this IEnumerable<EventStreamEvent> eventStream)
        {
            return eventStream.Select(x => x.Event);
        }

        public static List<EventStreamEvent> ToEventStreamEvents(this IEnumerable<IEvent> events)
        {
            return events.Select(x => new EventStreamEvent(x)).ToList();
        }
    }
}
