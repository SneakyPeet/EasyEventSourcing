using System;
using NUnit.Framework;
using SimpleEventSourcing.Tests.Domain.Helpers;
using SimpleEventSourcing.Messages.ShoppingCart;

namespace SimpleEventSourcing.Tests.Domain.ShoppingCart
{
    class ShoppingCartTests : Spesification
    {
        private readonly Guid cartId = new Guid("809b71b5-1fc5-4039-b7fe-5d23aa58c5b4");
        private readonly Guid clientId = new Guid("dfeeb7b5-1fc5-4039-b7fe-5d23aa58c5b4");

        [Test]
        public void CreateCart()
        {
            When(new CreateNewCart(cartId, clientId));
            Then(Expected.Event(new CartCreated(cartId, clientId)));
        }
    }

}
