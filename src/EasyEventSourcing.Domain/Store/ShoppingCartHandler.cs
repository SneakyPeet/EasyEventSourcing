using EasyEventSourcing.EventSourcing;
using EasyEventSourcing.Messages.Store;

namespace EasyEventSourcing.Domain.Store
{
    public class ShoppingCartHandler
        : ICommandHandler<CreateNewCart>
        , ICommandHandler<AddProductToCart>
    {
        private readonly IRepository repo;
        public ShoppingCartHandler(IRepository repo)
        {
            this.repo = repo;
        }
        public void Handle(CreateNewCart cmd)
        {
            this.repo.Save(ShoppingCart.Create(cmd.CartId, cmd.ClientId));
        }

        public void Handle(AddProductToCart cmd)
        {
            var cart = this.repo.GetById<ShoppingCart>(cmd.CartId);
            cart.AddProduct(cmd.ProductId, cmd.Price);
            this.repo.Save(cart);
        }
    }
}
