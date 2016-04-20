using EasyEventSourcing.Application.Read;
using EasyEventSourcing.Application.Write;
using EasyEventSourcing.Data;
using EasyEventSourcing.Data.MongoDb;
using EasyEventSourcing.EventSourcing.Handlers;

namespace EasyEventSourcing.Application
{
    public class Bootstrapper
    {
        public static ICommandDispatcher Bootstrap()
        {
            var eventHandlerFactory = new EventHandlerFactory(new MongoDb());
            var eventDispatcher = new EventDispatcher(eventHandlerFactory);
            var store = new InMemoryEventStore(eventDispatcher);
            var handlerFactory = new CommandHandlerFactory(store);
            return new CommandDispatcher(handlerFactory);
        }
    }
}
