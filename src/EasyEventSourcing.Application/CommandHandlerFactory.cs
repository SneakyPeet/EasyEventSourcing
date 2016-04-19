using EasyEventSourcing.Domain.Store;
using System;
using System.Collections.Generic;
using EasyEventSourcing.EventSourcing;
using EasyEventSourcing.EventSourcing.Exceptions;
using EasyEventSourcing.Messages;
using EasyEventSourcing.Messages.Store;

namespace EasyEventSourcing.Application
{
    public class CommandHandlerFactory : ICommandHandlerFactory
    {
        private readonly Dictionary<Type, Func<IHandler>> handlers = new Dictionary<Type, Func<IHandler>>();

        public CommandHandlerFactory(IEventStore eventStore)
        {
            Func<IRepository> newTransientRepo = () => new Repository(eventStore);

            RegisterTypesWithHandler(
                () => new ShoppingCartHandler(newTransientRepo()),
                typeof(CreateNewCart), typeof(AddProductToCart), typeof(RemoveProductFromCart), typeof(EmptyCart), typeof(Checkout));
        }

        private void RegisterTypesWithHandler(Func<IHandler> handler, params Type[] types)
        {
            foreach(var type in types)
            {
                this.handlers.Add(type, handler);
            }
        }

        public ICommandHandler<TCommand> Resolve<TCommand>() where TCommand : ICommand
        {
            if(this.handlers.ContainsKey(typeof(TCommand)))
            {
                var handler = this.handlers[typeof(TCommand)]() as ICommandHandler<TCommand>;
                if (handler != null)
                {
                    return handler;
                }
            }
            throw new NoCommandHandlerRegisteredException(typeof (TCommand));
        }
    }
}