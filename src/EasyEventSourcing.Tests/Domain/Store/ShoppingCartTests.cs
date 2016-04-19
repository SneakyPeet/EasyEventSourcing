using System;
using EasyEventSourcing.Domain.Store;
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
        public void CreateCart()
        {
            this.When(new CreateNewCart(this.cartId, this.clientId));
            this.Then(Expected.Event(new CartCreated(this.cartId, this.clientId)));
        }

        [Test]
        public void AddItem()
        {
            this.Given<ShoppingCart, CartCreated>(this.cartId, new CartCreated(this.cartId, this.clientId));
            this.When(new AddProductToCart(this.cartId, this.productId, 10));
            this.Then(Expected.Event(new ProductAddedToCart(this.cartId, this.productId, 10)));
        }

        [Test]
        public void NoCart()
        {
            this.When(new AddProductToCart(this.cartId, this.productId, 10));
            this.Throw<Exception>(Expected.Event(new ProductAddedToCart(this.cartId, this.productId, 10)));
        }
    }

}
