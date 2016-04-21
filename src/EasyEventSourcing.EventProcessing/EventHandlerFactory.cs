using System;
using System.Collections.Generic;
using EasyEventSourcing.Data.MongoDb;
using EasyEventSourcing.Domain.Shipping;
using EasyEventSourcing.EventSourcing.Handlers;
using EasyEventSourcing.EventSourcing.Persistence;
using EasyEventSourcing.Messages;
using System.Linq;
using EasyEventSourcing.Messages.Orders;
using EasyEventSourcing.Messages.Store;

namespace EasyEventSourcing.EventProcessing
{
    public class EventHandlerFactory : IEventHandlerFactory
    {
        private readonly Dictionary<Type, List<Func<EventsHandler>>> handlerFactories = new Dictionary<Type, List<Func<EventsHandler>>>();

        public EventHandlerFactory(IEventStore eventStore, ICommandDispatcher dispatcher, MongoDb mongo)
        {
            RegisterHandlerFactories(eventStore,dispatcher,mongo);
        }

        private void RegisterHandlerFactories(IEventStore eventStore, ICommandDispatcher dispatcher, MongoDb mongo)
        {
            this.RegisterHandlerFactoryWithTypes(
                () => new ShoppingCartEventHandler(mongo),
                typeof(CartCreated), typeof(ProductAddedToCart), typeof(ProductRemovedFromCart), typeof(CartEmptied), typeof(CartCheckedOut));

            this.RegisterHandlerFactoryWithTypes(
                () => new OrderEventHandler(new Repository(eventStore), dispatcher),
                typeof(OrderCreated), typeof(PaymentReceived), typeof(ShippingAddressConfirmed));
        }

        private void RegisterHandlerFactoryWithTypes(Func<EventsHandler> handler, params Type[] types)
        {
            foreach (var type in types)
            {
                this.handlerFactories.Add(type, new List<Func<EventsHandler>> { handler });
            }
        }

        public IEnumerable<EventsHandler> Resolve(IEvent evt)
        {
            var evtType = evt.GetType();
            if (this.handlerFactories.ContainsKey(evtType))
            {
                var factories = this.handlerFactories[evtType];
                return factories.Select(h => h());
            }
            return new List<EventsHandler>();
        }
    }
}