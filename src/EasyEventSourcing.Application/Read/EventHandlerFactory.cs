using System;
using System.Collections.Generic;
using EasyEventSourcing.Data.MongoDb;
using EasyEventSourcing.EventSourcing.Handlers;
using EasyEventSourcing.Messages;
using System.Linq;
using EasyEventSourcing.Messages.Store;

namespace EasyEventSourcing.Application.Read
{
    public class EventHandlerFactory : IEventHandlerFactory
    {
        private readonly Dictionary<Type, List<Func<IHandler>>> handlerFactories = new Dictionary<Type, List<Func<IHandler>>>();

        public EventHandlerFactory(MongoDb mongoDb)
        {
            RegisterHandlerFactoryWithTypes(
                () => new ShoppingCartEventHandler(mongoDb),
                typeof(CartCreated), typeof(ProductAddedToCart), typeof(ProductRemovedFromCart), typeof(CartEmptied), typeof(CartCheckedOut));
        }

        private void RegisterHandlerFactoryWithTypes(Func<IHandler> handler, params Type[] types)
        {
            foreach (var type in types)
            {
                this.handlerFactories.Add(type, new List<Func<IHandler>>{handler});
            }
        }

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