using System;
using EasyEventSourcing.EventSourcing.Domain;
using EasyEventSourcing.EventSourcing.Handlers;
using EasyEventSourcing.Messages.Orders;
using EasyEventSourcing.Messages.Shipping;

namespace EasyEventSourcing.Domain.Shipping
{
    public class ShippingSaga : Saga
    {
        protected override void RegisterAppliers()
        {
            RegisterApplier<StartedShippingProcess>(this.Apply);
            RegisterApplier<PaymentConfirmed>(this.Apply);
            RegisterApplier<AddressConfirmed>(this.Apply);
            RegisterApplier<OrderDelivered>(this.Apply);
        }

        private enum Status
        {
            Started,
            PaymentReceived,
            AddressReceived,
            ReadyToComplete,
            Complete
        }

        private Status status = Status.Started;

        public ShippingSaga() { }

        public static ShippingSaga Create(Guid orderId)
        {
            return new ShippingSaga(orderId);
        }

        private ShippingSaga(Guid orderId)
        {
            ApplyChanges(new StartedShippingProcess(orderId));
        }

        private void Apply(StartedShippingProcess evt)
        {
            this.id = evt.OrderId;
        }

        public void ConfirmPayment(ICommandDispatcher dispatcher)
        {
            if (AwaitingPayment())
            {
                ApplyChanges(new PaymentConfirmed(this.id));
                CompleteIfPossible(dispatcher);
            }
        }

        private bool AwaitingPayment()
        {
            return this.status == Status.Started || this.status == Status.AddressReceived;
        }

        private void Apply(PaymentConfirmed evt)
        {
            status = status == Status.AddressReceived ? Status.ReadyToComplete : Status.PaymentReceived;
        }

        public void ConfirmAddress(ICommandDispatcher dispatcher)
        {
            if (AwaitingAddress())
            {
                ApplyChanges(new AddressConfirmed(this.id));
                CompleteIfPossible(dispatcher);
            }
        }

        private bool AwaitingAddress()
        {
            return this.status == Status.Started || this.status == Status.PaymentReceived;
        }

        private void Apply(AddressConfirmed evt)
        {
            status = status == Status.PaymentReceived ? Status.ReadyToComplete : Status.AddressReceived;
        }

        private void CompleteIfPossible(ICommandDispatcher dispatcher)
        {
            if(status == Status.ReadyToComplete)
            {
                ApplyChanges(new OrderDelivered(this.id));
                dispatcher.Send(new CompleteOrder(this.id)); //todo this is wierd should do it after events have been persisted
            }
        }

        private void Apply(OrderDelivered obj)
        {
            status = Status.Complete;
        }
    }
}
