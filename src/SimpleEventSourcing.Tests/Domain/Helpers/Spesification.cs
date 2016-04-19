using System;
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

        protected void And<TEventStream, TEvent>(Guid id, TEvent evt)
            where TEvent : IEvent
            where TEventStream : EventStream, new()
        {
            Given<TEventStream, TEvent>(id, evt);
        }

        protected void Given<TEventStream, TEvent>(Guid id, TEvent evt) 
                                                    where TEvent : IEvent
                                                    where TEventStream : EventStream
        {
            var streamIdentifier = new StreamIdentifier(typeof(TEventStream).Name, id);
            var eventStoreStream = new EventStoreStream(streamIdentifier, new List<IEvent>{evt});
            this.eventStore.Save(new List<EventStoreStream>{eventStoreStream});
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

        protected void Throw<TException>(IEnumerable<IEvent> expectedEvents) where TException : Exception
        {
            Assert.Throws<TException>(
                () => Then(expectedEvents)
            );
        }

        [TearDown]
        public void TearDown()
        {
            eventStore.Complete();
        }
    }
}
