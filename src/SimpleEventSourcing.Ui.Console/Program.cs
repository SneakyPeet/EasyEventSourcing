using SimpleEventSourcing.Application;
using SimpleEventSourcing.Messages.ShoppingCart;
using System;
namespace SimpleEventSourcing.Ui.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var app = Bootstrapper.Bootstrap();

                app.Send(new CreateNewCart(Guid.NewGuid(), Guid.NewGuid()));
            }
            catch(Exception e)
            {
                System.Console.WriteLine(e.Message);
            }
        }
    }
}
