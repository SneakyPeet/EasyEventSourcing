using EasyEventSourcing.Data;

namespace EasyEventSourcing.Application
{
    public class Bootstrapper
    {
        public static ICommandDispatcher Bootstrap()
        {
            var store = new InMemoryEventStore();
            var handlerFactory = new CommandHandlerFactory(store);
            return new CommandDispatcher(handlerFactory);
        }
    }
}
