using EasyEventSourcing.Application.Read;
using EasyEventSourcing.Application.Write;
using EasyEventSourcing.Data;
using EasyEventSourcing.Data.MongoDb;
using EasyEventSourcing.Messages;

namespace EasyEventSourcing.Application
{
    public class Bootstrapper
    {
        public static PretendApplication Bootstrap()
        {
            var mongoDb = new MongoDb();
            var eventHandlerFactory = new EventHandlerFactory(mongoDb);
            var eventDispatcher = new EventDispatcher(eventHandlerFactory);
            var store = new InMemoryEventStore(eventDispatcher);
            var handlerFactory = new CommandHandlerFactory(store);
            var dispatcher =  new CommandDispatcher(handlerFactory);
            return new PretendApplication(mongoDb, dispatcher);
        }

        public class PretendApplication
        {
            public PretendApplication(MongoDb mongoDb, CommandDispatcher dispatcher)
            {
                this.MongoDb = mongoDb;
                this.dispatcher = dispatcher;
            }

            private readonly CommandDispatcher dispatcher;
            public MongoDb MongoDb { get; private set; }

            public void Send<TCommand>(TCommand cmd) where TCommand : ICommand
            {
                dispatcher.Send(cmd);
            }
        }
    }
}
