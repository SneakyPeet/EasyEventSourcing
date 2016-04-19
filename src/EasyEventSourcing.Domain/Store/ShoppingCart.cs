using System;
using System.Collections.Generic;
using EasyEventSourcing.EventSourcing;
using EasyEventSourcing.Messages.Store;

namespace EasyEventSourcing.Domain.Store
{
    public class ShoppingCart : Aggregate
    {
        public ShoppingCart() {}

        private readonly Dictionary<Guid, Decimal> products = new Dictionary<Guid, decimal>();
        
        private ShoppingCart(Guid cartId, Guid customerId)
        {
            this.ApplyChanges(new CartCreated(cartId, customerId));
        }

        protected override void RegisterAppliers()
        {
            this.RegisterApplier<CartCreated>(this.Apply);
            this.RegisterApplier<ProductAddedToCart>(this.Apply);
        }

        public static ShoppingCart Create(Guid cartId, Guid customerId)
        {
            return new ShoppingCart(cartId, customerId);
        }

        private void Apply(CartCreated evt)
        {
            this.id = evt.CartId;
        }

        public void AddProduct(Guid productId, decimal price)
        {
            if (!products.ContainsKey(productId))
            {
                this.ApplyChanges(new ProductAddedToCart(productId, price));
            }
        }

        private void Apply(ProductAddedToCart evt)
        {
            products.Add(evt.ProductId, evt.Price);
        }
    }
}
