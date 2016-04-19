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

            Func<ShoppingCartHandler> newTransientShoppingCartHandler = () => new ShoppingCartHandler(newTransientRepo());
            this.handlers.Add(typeof(CreateNewCart), newTransientShoppingCartHandler);
            this.handlers.Add(typeof(AddProductToCart), newTransientShoppingCartHandler);
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