using SimpleEventSourcing.Messages.Store;
using SimpleEventSourcing.EventSourcing;
using System;

namespace SimpleEventSourcing.Domain.Store
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
            repo.Save(ShoppingCart.Create(message.CartId, message.ClientId));
        }
    }
}
