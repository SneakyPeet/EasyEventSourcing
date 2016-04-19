using EasyEventSourcing.EventSourcing;
using EasyEventSourcing.Messages.Store;

namespace EasyEventSourcing.Domain.Store
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
            this.repo.Save(ShoppingCart.Create(message.CartId, message.ClientId));
        }
    }
}
