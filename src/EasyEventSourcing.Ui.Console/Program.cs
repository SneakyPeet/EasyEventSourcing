using EasyEventSourcing.Application;
using System;
using EasyEventSourcing.Messages.Store;

namespace EasyEventSourcing.Ui.Console
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
