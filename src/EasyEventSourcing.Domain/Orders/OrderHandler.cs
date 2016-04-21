using EasyEventSourcing.EventSourcing.Handlers;
using EasyEventSourcing.EventSourcing.Persistence;
using EasyEventSourcing.Messages.Orders;
using EasyEventSourcing.Messages.Shipping;

namespace EasyEventSourcing.Domain.Orders
{
    public class OrderHandler
        : ICommandHandler<PayForOrder>
          , ICommandHandler<ConfirmShippingAddress>
          , ICommandHandler<CompleteOrder>
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

        public void Handle(CompleteOrder cmd)
        {
            var order = this.repository.GetById<Order>(cmd.OrderId);
            order.CompleteOrder();
            this.repository.Save(order);
        }
    }
}