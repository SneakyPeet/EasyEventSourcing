using System.Collections.Generic;
using NUnit.Framework;
using SharpTestsEx;
using SimpleEventSourcing.EventSourcing;
using SimpleEventSourcing.Application;
using SimpleEventSourcing.Messages;

namespace SimpleEventSourcing.Tests.Domain.Helpers
{
    class Spesification
    {
        protected readonly ICommandDispatcher app;
        protected readonly TestEventStore eventStore;

        protected Spesification()
        {
            eventStore = new TestEventStore();
            var commandHandlerFactory = new CommandHandlerFactory(eventStore);
            app = new CommandDispatcher(commandHandlerFactory);
        }
        
        protected void And<T>(T command) where T : ICommand
        {
            Given<T>(command);
        }

        protected void Given<T>(T command) where T : ICommand
        {
            this.app.Send<T>(command);
            this.eventStore.CommitInitialEvents();
        }

        protected void When<T>(T command) where T : ICommand
        {
            this.app.Send<T>(command);
        }

        protected void Then(IEnumerable<IEvent> expectedEvents)
        {
            this.eventStore.NewEvents().Should()
                      .Not.Be.Empty()
                      .And.Have.SameSequenceAs(expectedEvents)
                      .And.Have.UniqueValues();
        }

        protected void ThenNoEvents()
        {
            this.eventStore.NewEvents().Should()
                .Be.Empty();
        }

        [TearDown]
        public void TearDown()
        {
            eventStore.Complete();
        }
    }
}
