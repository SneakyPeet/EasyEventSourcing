using System;

namespace EasyEventSourcing.Data.MongoDb.ReadModels
{
    public class ShoppingCartItemReadModel
    {
        public Guid ProductId { get; set; }
        public Decimal Price { get; set; }
    }
}