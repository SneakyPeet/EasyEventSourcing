using System;
using SimpleEventSourcing.EventSourcing;
using SimpleEventSourcing.Messages.Store;

namespace SimpleEventSourcing.Domain.Store
{
    public class ShoppingCart : Aggregate
    {
        protected override void RegisterAppliers()
        {
            RegisterApplier<CartCreated>(this.Apply);
        }

        private ShoppingCart(Guid cartId, Guid customerId)
        {
            this.ApplyChanges(new CartCreated(cartId, customerId));
        }

        public static ShoppingCart Create(Guid cartId, Guid customerId)
        {
            return new ShoppingCart(cartId, customerId);
        }

        private void Apply(CartCreated evt)
        {
            this.id = evt.CartId;
        }
    }
}
