using EasyEventSourcing.EventSourcing.Handlers;
using EasyEventSourcing.Messages;

namespace EasyEventSourcing.Application.Read
{
    public class EventDispatcher : IEventDispatcher
    {
        private readonly IEventHandlerFactory factory;

        public EventDispatcher(IEventHandlerFactory factory)
        {
            this.factory = factory;
        }
        public void Send(IEvent evt)
        {
            var handlers = this.factory.Resolve(evt);
            foreach (var eventHandler in handlers)
            {
                eventHandler.HandleEvent(evt);
            }
        }
    }
}