using SimpleEventSourcing.Messages.ShoppingCart;
using SimpleEventSourcing.EventSourcing;
using System;

namespace SimpleEventSourcing.Domain.ShoppingCart
{
    public class ShoppingCartCommandHandler : ICommandHandler<CreateNewCart>
    {
        private readonly IRepository repo;
        public ShoppingCartCommandHandler(IRepository repo)
        {
            this.repo = repo;
        }
        public void Handle(CreateNewCart message)
        {
            throw new NotImplementedException();
        }
    }
}
