using EasyEventSourcing.EventSourcing.Handlers;
using EasyEventSourcing.EventSourcing.Persistence;
using EasyEventSourcing.Messages.Orders;

namespace EasyEventSourcing.Domain.Shipping
{
    public class OrderEventHandler : EventsHandler
    {
        private readonly IRepository repo;
        private readonly ICommandDispatcher dispatcher;

        public OrderEventHandler(IRepository repo, ICommandDispatcher dispatcher)
        {
            this.repo = repo;
            this.dispatcher = dispatcher;
        }

        protected override void RegisterHandlers()
        {
            this.RegisterHandler<OrderCreated>(this.Handle);
            this.RegisterHandler<PaymentReceived>(this.Handle);
            this.RegisterHandler<ShippingAddressConfirmed>(this.Handle);
        }

        private void Handle(OrderCreated evt)
        {
            var saga = ShippingSaga.Create(evt.OrderId);
            repo.Save(saga);
        }

        private void Handle(PaymentReceived evt)
        {
            var saga = this.repo.GetById<ShippingSaga>(evt.OrderId);
            saga.ConfirmPayment(dispatcher);
            this.repo.Save(saga);
        }

        private void Handle(ShippingAddressConfirmed evt)
        {
            var saga = this.repo.GetById<ShippingSaga>(evt.OrderId);
            saga.ConfirmAddress(dispatcher);
            this.repo.Save(saga);
        }
    }
}