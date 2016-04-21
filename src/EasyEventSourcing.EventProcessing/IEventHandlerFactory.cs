using System.Collections.Generic;
using EasyEventSourcing.EventSourcing.Handlers;
using EasyEventSourcing.Messages;

namespace EasyEventSourcing.EventProcessing
{
    public interface IEventHandlerFactory
    {
        IEnumerable<EventsHandler> Resolve(IEvent evt);
    }
}