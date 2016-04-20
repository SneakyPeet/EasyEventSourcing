using System;
using System.Collections.Generic;
using EasyEventSourcing.EventSourcing.Handlers;
using EasyEventSourcing.Messages;
using System.Linq;

namespace EasyEventSourcing.Application.Read
{
    public class EventHandlerFactory : IEventHandlerFactory
    {
        private readonly Dictionary<Type, List<Func<IHandler>>> handlerFactories = new Dictionary<Type, List<Func<IHandler>>>();

        public IEnumerable<IEventHandler<TEvent>> Resolve<TEvent>() where TEvent : IEvent
        {
            if (this.handlerFactories.ContainsKey(typeof(TEvent)))
            {
                var factories = this.handlerFactories[typeof(TEvent)];
                return factories.Select(h => h() as IEventHandler<TEvent>);
            }
            return new List<IEventHandler<TEvent>>();
        }
    }
}