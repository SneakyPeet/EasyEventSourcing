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
            this.GivenNewCart();
            this.When(new AddProductToCart(this.cartId, this.productId, productPrice));
            this.Then(new ProductAddedToCart(this.cartId, this.productId, productPrice));
        }

        [Test]
        public void RemovingAProduct()
        {
            this.GivenNewCart();
            this.AndAddedProduct();
            this.When(new RemoveProductFromCart(this.cartId, this.productId));
            this.Then(new ProductRemovedFromCart(this.cartId, this.productId));
        }

        [Test]
        public void EmptyCart()
        {
            this.GivenNewCart();
            this.AndAddedProduct();
            this.And<ShoppingCart, ProductAddedToCart>(this.cartId, new ProductAddedToCart(this.cartId, Guid.NewGuid(), productPrice));
            this.When(new EmptyCart(this.cartId));
            this.Then(new CartEmptied(this.cartId));
        }

        [Test]
        public void CheckingOut()
        {
            this.GivenNewCart();
            this.AndAddedProduct();
            this.When(new Checkout(this.cartId));
            this.Then(
                new CartCheckedOut(this.cartId),
                new OrderCreated(this.cartId, this.clientId, new []
                                                                 {
                                                                     new OrderItem(this.productId,productPrice)
                                                                 })    
            );
        }

        [Test]
        public void NoCart()
        {
            Assert.Throws<Exception>(
                () => this.When(new AddProductToCart(this.cartId, this.productId, productPrice))
            );
        }

        private void GivenNewCart()
        {
            this.Given<ShoppingCart, CartCreated>(this.cartId, new CartCreated(this.cartId, this.clientId));
        }

        private void AndAddedProduct()
        {
            this.And<ShoppingCart, ProductAddedToCart>(this.cartId, new ProductAddedToCart(this.cartId, this.productId, productPrice));
        }
    }

}
