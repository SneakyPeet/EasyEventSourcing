using System.Collections.Generic;
using EasyEventSourcing.EventSourcing.Handlers;
using EasyEventSourcing.Messages;

namespace EasyEventSourcing.Application.Read
{
    public interface IEventHandlerFactory
    {
        IEnumerable<EventsHandler> Resolve(IEvent evt);
    }
}