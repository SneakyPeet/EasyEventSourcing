using EasyEventSourcing.Application;
using System;
using EasyEventSourcing.Messages.Store;
using EasyEventSourcing.Messages.Orders;

namespace EasyEventSourcing.Ui.Console
{
    class Program
    {
        private static readonly Guid clientId = Guid.NewGuid();
        private static readonly Guid cartId = Guid.NewGuid();

        static void Main(string[] args)
        {
            try
            {
                var app = Bootstrapper.Bootstrap();

                if(!app.MongoDb.HasCart(clientId))
                {
                    app.Send(new CreateNewCart(cartId, clientId));
                }

                app.Send(new AddProductToCart(cartId, Guid.NewGuid(), 50));

                app.Send(new AddProductToCart(cartId, Guid.NewGuid(), 10));

                var cartModel = app.MongoDb.GetCartById(cartId);

                app.Send(new Checkout(cartId));
                var hasCartBeenRemovedAfterCheckout = app.MongoDb.HasCart(clientId);

                var orderId = cartId;
                app.Send(new ConfirmShippingAddress(orderId, "My Home"));
                app.Send(new PayForOrder(orderId));

                
            }
            catch(Exception e)
            {
                System.Console.WriteLine(e.Message);
            }
        }
    }
}
