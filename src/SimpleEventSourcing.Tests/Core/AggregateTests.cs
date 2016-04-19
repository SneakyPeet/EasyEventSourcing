using NUnit.Framework;
using SimpleEventSourcing.Tests.Core.Helpers;
using SimpleEventSourcing.Messages;
using SimpleEventSourcing.EventSourcing.Exceptions;
using System.Linq;
using System.Collections.Generic;

namespace SimpleEventSourcing.Tests.Core
{
    [TestFixture]
    public class AggregateTests
    {
        private TestAggregate aggregate;

        [SetUp]
        public void SetUp()
        {
            this.aggregate = new TestAggregate();
        }

        [Test]
        public void AggregateNameShouldEqualTheConcreteImplementationClassName()
        {
            Assert.AreEqual("TestAggregate", this.aggregate.Name);
        }

        [Test]
        public void ApplyingAnEventShouldAddItToUncommitedEvents()
        {
            this.aggregate.ExecuteTest();
            var evt = this.aggregate.GetUncommitedChanges().Single();
            Assert.IsInstanceOf(typeof(TestEvent), evt);
        }

        [Test]
        public void MarkingEventsAsCommitedShouldClearUncommitedEvents()
        {
            this.aggregate.ExecuteTest();
            this.aggregate.ExecuteTest();
            this.aggregate.Commit();
            Assert.IsEmpty(this.aggregate.GetUncommitedChanges());
        }

        [Test]
        public void ApplyFromCommandShouldDoValidation()
        {
            this.aggregate.ExecuteTest();
            Assert.IsTrue(this.aggregate.Validation);
            Assert.IsTrue(this.aggregate.StateUpdated);
        }

        [Test]
        public void ApplyFromHistoryShouldNotDoValidation()
        {
            this.aggregate.LoadFromHistory(new List<IEvent> { new TestEvent() });
            Assert.IsFalse(this.aggregate.Validation);
            Assert.IsTrue(this.aggregate.StateUpdated);
        }

        [Test]
        public void EventsNotRegisteredShouldThrowException()
        {
            Assert.Throws<NoEventApplyMethodRegisteredException>(
                () => this.aggregate.LoadFromHistory(new List<IEvent> { new InvalidEvent() })
            );
        }
    }
}
