using System;
using SimpleEventSourcing.EventSourcing;
using SimpleEventSourcing.Messages.ShoppingCart;

namespace SimpleEventSourcing.Domain.ShoppingCart
{
    public class ShoppingCart : Aggregate
    {
        
        public override string Name
        {
            get
            {
                return "ShoppingCart";
            }
        }

        protected override void RegisterAppliers()
        {
            this.eventAppliers.Add(typeof(CartCreated), (e) => Apply((CartCreated)e));
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
