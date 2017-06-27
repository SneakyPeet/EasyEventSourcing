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
            var process = ShippingProcess.Create(evt.OrderId);
            repo.Save(process);
        }

        public void Handle(PaymentReceived evt)
        {
            var process = this.repo.GetById<ShippingProcess>(evt.OrderId);
            process.ConfirmPayment(dispatcher);
            this.repo.Save(process);
        }

        public void Handle(ShippingAddressConfirmed evt)
        {
            var process = this.repo.GetById<ShippingProcess>(evt.OrderId);
            process.ConfirmAddress(dispatcher);
            this.repo.Save(process);
        }
    }
}