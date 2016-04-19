using System;
using System.Collections.Generic;
using EasyEventSourcing.Domain.Store;
using EasyEventSourcing.Messages;
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
            this.When(new AddProductToCart(this.cartId, this.productId, 10));
            this.Then(new ProductAddedToCart(this.cartId, this.productId, 10));
        }

        [Test]
        public void RemovingAProduct()
        {
            this.Given<ShoppingCart, CartCreated>(this.cartId, new CartCreated(this.cartId, this.clientId));
            this.Given<ShoppingCart, ProductAddedToCart>(this.cartId, new ProductAddedToCart(this.cartId, this.productId, 10));
            this.When(new RemoveProductFromCart(this.cartId, this.productId));
            this.Then(new ProductRemovedFromCart(this.cartId, this.productId));
        }

        [Test]
        public void CheckingOut()
        {
            this.Given<ShoppingCart, CartCreated>(this.cartId, new CartCreated(this.cartId, this.clientId));
            this.Given<ShoppingCart, ProductAddedToCart>(this.cartId, new ProductAddedToCart(this.cartId, this.productId, 10));
            this.When(new Checkout(this.cartId));
            this.Then(new ProductRemovedFromCart(this.cartId, this.productId));
        }

        [Test]
        public void NoCart()
        {
            this.When(new AddProductToCart(this.cartId, this.productId, 10));
            this.ThrowsWhen<Exception, AddProductToCart>(new AddProductToCart(this.cartId, this.productId, 10));
        }
    }

}
