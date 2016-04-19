using System;
using EasyEventSourcing.Domain.Store;
using EasyEventSourcing.Messages.Orders;
using EasyEventSourcing.Messages.Store;
using EasyEventSourcing.Tests.Domain.Helpers;
using NUnit.Framework;

namespace EasyEventSourcing.Tests.Domain.Store
{
    class ShoppingCartTests : Spesification
    {
        private readonly Guid cartId = Guid.NewGuid();
        private readonly Guid clientId = Guid.NewGuid();
        private readonly Guid productId = Guid.NewGuid();
        private const Decimal productPrice = 10;

        [Test]
        public void CartCreation()
        {
            this.When(new CreateNewCart(this.cartId, this.clientId));
            this.Then(new CartCreated(this.cartId, this.clientId));
        }

        [Test]
        public void AddingAProduct()
        {
            this.Given<ShoppingCart, CartCreated>(this.cartId, new CartCreated(this.cartId, this.clientId));
            this.When(new AddProductToCart(this.cartId, this.productId, productPrice));
            this.Then(new ProductAddedToCart(this.productId, productPrice));
        }

        [Test]
        public void RemovingAProduct()
        {
            this.Given<ShoppingCart, CartCreated>(this.cartId, new CartCreated(this.cartId, this.clientId));
            this.And<ShoppingCart, ProductAddedToCart>(this.cartId, new ProductAddedToCart(this.productId, productPrice));
            this.When(new RemoveProductFromCart(this.cartId, this.productId));
            this.Then(new ProductRemovedFromCart(this.productId));
        }

        [Test]
        public void EmptyCart()
        {
            this.Given<ShoppingCart, CartCreated>(this.cartId, new CartCreated(this.cartId, this.clientId));
            this.And<ShoppingCart, ProductAddedToCart>(this.cartId, new ProductAddedToCart(this.productId, productPrice));
            this.And<ShoppingCart, ProductAddedToCart>(this.cartId, new ProductAddedToCart(Guid.NewGuid(), productPrice));
            this.When(new EmptyCart(this.cartId));
            this.Then(new CartEmptied());
        }

        [Test]
        public void CheckingOut()
        {
            this.Given<ShoppingCart, CartCreated>(this.cartId, new CartCreated(this.cartId, this.clientId));
            this.And<ShoppingCart, ProductAddedToCart>(this.cartId, new ProductAddedToCart(this.productId, productPrice));
            this.When(new Checkout(this.cartId));
            this.Then(
                new CartCheckedOut(),
                new OrderCreated(this.cartId, this.clientId, new []
                                                                 {
                                                                     new OrderItem(this.productId,productPrice)
                                                                 })    
            );
        }

        [Test]
        public void NoCart()
        {
            this.When(new AddProductToCart(this.cartId, this.productId, productPrice));
            this.ThrowsWhen<Exception, AddProductToCart>(new AddProductToCart(this.cartId, this.productId, productPrice));
        }
    }

}
