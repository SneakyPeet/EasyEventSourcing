using System.Linq;
using EasyEventSourcing.Data.MongoDb.ReadModels;
using EasyEventSourcing.EventSourcing.Handlers;
using EasyEventSourcing.Messages.Store;

namespace EasyEventSourcing.Data.MongoDb
{
    public class ShoppingCartEventHandler 
        : IEventHandler<CartCreated>
        , IEventHandler<ProductAddedToCart>
        , IEventHandler<ProductRemovedFromCart>
        , IEventHandler<CartEmptied>
        , IEventHandler<CartCheckedOut>
    {
        private readonly MongoDb db;

        public ShoppingCartEventHandler(MongoDb db)
        {
            this.db = db;
        }

        public void Handle(CartCreated evt)
        {
            var newCart = new ShoppingCartReadModel
                              {
                                  ClientId = evt.ClientId,
                                  Id = evt.CartId
                              };
            db.SaveCart(newCart);
        }

        public void Handle(ProductAddedToCart evt)
        {
            var cart = db.GetCartById(evt.CartId);
            var product = cart.Items.FirstOrDefault(x => x.ProductId == evt.ProductId);
            if (product != null)
            {
                product.Price = evt.Price;
            }
            else
            {
                cart.Items.Add(new ShoppingCartItemReadModel
                                   {
                                       Price = evt.Price,
                                       ProductId = evt.ProductId
                                   });
            }
            db.SaveCart(cart);
        }

        public void Handle(ProductRemovedFromCart evt)
        {
            var cart = db.GetCartById(evt.CartId);
            cart.Items.RemoveAll(x => x.ProductId == evt.ProductId);
            db.SaveCart(cart);
        }

        public void Handle(CartEmptied evt)
        {
            var cart = db.GetCartById(evt.CartId);
            cart.Items.Clear();
            db.SaveCart(cart);
        }

        public void Handle(CartCheckedOut evt)
        {
            db.RemoveCart(evt.CartId);
        }
    }
}