using System;
using System.Collections.Generic;
using System.Linq;
using EasyEventSourcing.Data.MongoDb.ReadModels;

namespace EasyEventSourcing.Data.MongoDb
{
    public class MongoDb
    {
        private readonly Dictionary<Guid, ShoppingCartReadModel> carts = new Dictionary<Guid, ShoppingCartReadModel>();

        public ShoppingCartReadModel GetCartById(Guid id)
        {
            return this.carts[id];
        }

        public bool HasCart(Guid clientId)
        {
            return this.carts.Values.Any(x => x.ClientId == clientId);
        }

        public void SaveCart(ShoppingCartReadModel cart)
        {
            if (!this.carts.ContainsKey(cart.Id))
            {
                this.carts.Add(cart.Id, cart);
            }
            else
            {
                this.carts[cart.Id] = cart;
            }
        }

        public void RemoveCart(Guid cartId)
        {
            if (this.carts.ContainsKey(cartId))
            {
                this.carts.Remove(cartId);
            }
        }
    }
}