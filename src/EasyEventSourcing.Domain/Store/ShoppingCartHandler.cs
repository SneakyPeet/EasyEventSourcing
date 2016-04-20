using System;
using EasyEventSourcing.EventSourcing;
using EasyEventSourcing.EventSourcing.Handlers;
using EasyEventSourcing.EventSourcing.Persistence;
using EasyEventSourcing.Messages.Store;

namespace EasyEventSourcing.Domain.Store
{
    public class ShoppingCartHandler
        : ICommandHandler<CreateNewCart>
        , ICommandHandler<AddProductToCart>
        , ICommandHandler<RemoveProductFromCart>
        , ICommandHandler<EmptyCart>
        , ICommandHandler<Checkout>
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
            Execute(cmd.CartId, (cart) => cart.AddProduct(cmd.ProductId, cmd.Price));
        }

        public void Handle(RemoveProductFromCart cmd)
        {
            Execute(cmd.CartId, (cart) => cart.RemoveProduct(cmd.ProductId));
        }

        public void Handle(EmptyCart cmd)
        {
            Execute(cmd.CartId, (cart) => cart.Empty());
        }

        public void Handle(Checkout cmd)
        {
            var cart = this.repo.GetById<ShoppingCart>(cmd.CartId);
            var order = cart.Checkout();
            this.repo.Save(cart, order);
        }

        private void Execute(Guid id, Action<ShoppingCart> action)
        {
            var cart = this.repo.GetById<ShoppingCart>(id);
            action(cart);
            this.repo.Save(cart);
        }
    }
}
