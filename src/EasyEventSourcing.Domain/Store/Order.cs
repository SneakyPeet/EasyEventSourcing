using System;
using System.Collections.Generic;
using System.Linq;
using EasyEventSourcing.EventSourcing;
using EasyEventSourcing.EventSourcing.Domain;
using EasyEventSourcing.EventSourcing.Handlers;
using EasyEventSourcing.EventSourcing.Persistence;
using EasyEventSourcing.Messages.Orders;
using EasyEventSourcing.Messages.Shipping;

namespace EasyEventSourcing.Domain.Store
{
    public class Order : Aggregate
    {
        protected override void RegisterAppliers()
        {
            RegisterApplier<OrderCreated>(this.Apply);
            RegisterApplier<PaymentReceived>(this.Apply);
            RegisterApplier<ShippingAddressConfirmed>(this.Apply);
            RegisterApplier<OrderCompleted>(this.Apply);
        }

        private bool paidFor;
        private bool shippingAddressProvided;
        private bool completed;

        public static Order Create(Guid orderId, Guid clientId, IEnumerable<OrderItem> items)
        {
            return new Order(orderId, clientId, items);
        }

        private Order(Guid orderId, Guid clientId, IEnumerable<OrderItem> items)
        {
            ApplyChanges(new OrderCreated(orderId, clientId, items.ToArray()));
        }

        public Order()
        {
            throw new NotImplementedException();
        }

        private void Apply(OrderCreated evt)
        {
            this.id = evt.OrderId;
        }

        public void ProvideShippingAddress(string address)
        {
            if(!this.shippingAddressProvided && !this.completed)
            {
                ApplyChanges(new ShippingAddressConfirmed(this.id, address));
            }
        }

        private void Apply(ShippingAddressConfirmed evt)
        {
            this.shippingAddressProvided = true;
        }

        public void Pay()
        {
            if (!this.paidFor && !this.completed)
            {
                ApplyChanges(new PaymentReceived(this.id));
            }
        }

        public void Apply(PaymentReceived evt)
        {
            this.paidFor = true;
        }

        public void CompleteOrder()
        {
            if(!this.paidFor || !this.shippingAddressProvided)
            {
                throw new CannotCompleteOrderException();
            }
            this.ApplyChanges(new OrderCompleted(this.id));
        }

        private void Apply(OrderCompleted evt)
        {
            this.completed = true;
        }
    }

    public class OrderHandler
        : ICommandHandler<PayForOrder>
        , ICommandHandler<ConfirmShippingAddress>
        , ICommandHandler<ShipOrder>
    {
        private readonly IRepository repository;

        public OrderHandler(IRepository repository)
        {
            this.repository = repository;
        }

        public void Handle(PayForOrder cmd)
        {
            var order = this.repository.GetById<Order>(cmd.OrderId);
            order.Pay();
            this.repository.Save(order);
        }

        public void Handle(ConfirmShippingAddress cmd)
        {
            var order = this.repository.GetById<Order>(cmd.OrderId);
            order.ProvideShippingAddress(cmd.Address);
            this.repository.Save(order);
        }

        public void Handle(ShipOrder cmd)
        {
            var order = this.repository.GetById<Order>(cmd.OrderId);
            order.CompleteOrder();
            this.repository.Save(order);
        }
    }
}