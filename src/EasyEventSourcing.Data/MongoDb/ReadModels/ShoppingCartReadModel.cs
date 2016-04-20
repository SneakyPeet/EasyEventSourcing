using System;
using System.Collections.Generic;

namespace EasyEventSourcing.Data.MongoDb.ReadModels
{
    public class ShoppingCartReadModel
    {
        public ShoppingCartReadModel()
        {
            this.Items = new List<ShoppingCartItemReadModel>();
        }
        public List<ShoppingCartItemReadModel> Items { get; private set; }
        public Guid Id { get; set; }
        public Guid ClientId { get; set; }
    }
}