using System;
using System.Collections.Generic;
using EasyEventSourcing.EventSourcing;
using EasyEventSourcing.EventSourcing.Domain;
using EasyEventSourcing.Messages.Orders;
using EasyEventSourcing.Messages.Store;
using System.Linq;

namespace EasyEventSourcing.Domain.Store
{
    public class ShoppingCart : Aggregate
    {
        public ShoppingCart() {}

        private readonly Dictionary<Guid, Decimal> products = new Dictionary<Guid, decimal>();
        private bool checkedOut;
        private Guid clientId;

        private ShoppingCart(Guid cartId, Guid customerId)
        {
            this.ApplyChanges(new CartCreated(cartId, customerId));
        }

        protected override void RegisterAppliers()
        {
            this.RegisterApplier<CartCreated>(this.Apply);
            this.RegisterApplier<ProductAddedToCart>(this.Apply);
            this.RegisterApplier<ProductRemovedFromCart>(this.Apply);
            this.RegisterApplier<CartEmptied>(this.Apply);
            this.RegisterApplier<CartCheckedOut>(this.NoStateChange);
        }

        public static ShoppingCart Create(Guid cartId, Guid customerId)
        {
            return new ShoppingCart(cartId, customerId);
        }

        private void Apply(CartCreated evt)
        {
            this.id = evt.CartId;
            this.clientId = evt.ClientId;
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

        public void RemoveProduct(Guid productId)
        {
            if(products.ContainsKey(productId))
            {
                this.ApplyChanges(new ProductRemovedFromCart(productId));
            }
        }

        private void Apply(ProductRemovedFromCart evt)
        {
            products.Remove(evt.ProductId);
        }

        public void Empty()
        {
            this.ApplyChanges(new CartEmptied());
        }

        private void Apply(CartEmptied evt)
        {
            products.Clear();
        }

        public EventStream Checkout()
        {
            this.ApplyChanges(new CartCheckedOut());
            return Order.Create(this.id, this.clientId, this.products.Select(x => new OrderItem(x.Key, x.Value)));
        }
    }
}
