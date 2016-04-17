using System;
using SimpleEventSourcing.EventSourcing;

namespace SimpleEventSourcing.Tests.Core.Helpers
{
    class TestAggregate : Aggregate
    {
        public TestAggregate()
        {
            this.Validation = false;
            this.StateUpdated = false;
        }
        public override string Name { get { return "Test"; } }

        protected override void RegisterAppliers()
        {
            this.eventAppliers.Add(typeof(TestEvent), (evt) => Apply((TestEvent)evt));
        }

        public bool StateUpdated { get; internal set; }
        public bool Validation { get; internal set; }

        public void ExecuteTest()
        {
            this.Validation = true;
            this.ApplyChanges(new TestEvent());
        }

        private void Apply(TestEvent evt)
        {
            this.StateUpdated = true;
        }
    }


}
