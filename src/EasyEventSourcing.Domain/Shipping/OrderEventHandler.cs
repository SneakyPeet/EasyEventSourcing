using EasyEventSourcing.EventSourcing.Handlers;
using EasyEventSourcing.EventSourcing.Persistence;
using EasyEventSourcing.Messages.Orders;

namespace EasyEventSourcing.Domain.Shipping
{
    public class OrderEventHandler 
        : IEventHandler<OrderCreated>
        , IEventHandler<PaymentReceived>
        , IEventHandler<ShippingAddressConfirmed>
    {
        private readonly IRepository repo;
        private readonly ICommandDispatcher dispatcher;

        public OrderEventHandler(IRepository repo, ICommandDispatcher dispatcher)
        {
            this.repo = repo;
            this.dispatcher = dispatcher;
        }

        public void Handle(OrderCreated evt)
        {
            var saga = ShippingSaga.Create(evt.OrderId);
            repo.Save(saga);
        }

        public void Handle(PaymentReceived evt)
        {
            var saga = this.repo.GetById<ShippingSaga>(evt.OrderId);
            saga.ConfirmPayment(dispatcher);
            this.repo.Save(saga);
        }

        public void Handle(ShippingAddressConfirmed evt)
        {
            var saga = this.repo.GetById<ShippingSaga>(evt.OrderId);
            saga.ConfirmAddress(dispatcher);
            this.repo.Save(saga);
        }
    }
}