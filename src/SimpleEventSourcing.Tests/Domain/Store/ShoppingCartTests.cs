using System;
using NUnit.Framework;
using SimpleEventSourcing.Tests.Domain.Helpers;
using SimpleEventSourcing.Messages.Store;
using SimpleEventSourcing.Domain.Store;

namespace SimpleEventSourcing.Tests.Domain.Store
{
    class ShoppingCartTests : Spesification
    {
        private readonly Guid cartId = Guid.NewGuid();
        private readonly Guid clientId = Guid.NewGuid();
        private readonly Guid productId = Guid.NewGuid();

        [Test]
        public void CreateCart()
        {
            When(new CreateNewCart(cartId, clientId));
            Then(Expected.Event(new CartCreated(cartId, clientId)));
        }

        [Test]
        public void AddItem()
        {
            Given<ShoppingCart, CartCreated>(cartId, new CartCreated(cartId, clientId));
            When(new AddProductToCart(cartId, productId, 10));
            Then(Expected.Event(new ProductAddedToCart(cartId, productId, 10)));
        }

        [Test]
        public void NoCart()
        {
            When(new AddProductToCart(cartId, productId, 10));
            Throw<Exception>(Expected.Event(new ProductAddedToCart(cartId, productId, 10)));
        }
    }

}
