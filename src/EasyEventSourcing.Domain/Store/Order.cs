using System;
using System.Collections.Generic;
using System.Linq;
using EasyEventSourcing.EventSourcing;
using EasyEventSourcing.EventSourcing.Domain;
using EasyEventSourcing.Messages.Orders;

namespace EasyEventSourcing.Domain.Store
{
    public class Order : Aggregate
    {
        protected override void RegisterAppliers()
        {
            RegisterApplier<OrderCreated>(this.NoStateChange);
        }

        private Order(Guid orderId, Guid clientId, IEnumerable<OrderItem> items)
        {
            ApplyChanges(new OrderCreated(orderId, clientId, items.ToArray()));
        }

        public static Order Create(Guid orderId, Guid clientId, IEnumerable<OrderItem> items)
        {
            return new Order(orderId, clientId, items);
        }
    }
}