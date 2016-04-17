using System.Collections.Generic;
using SimpleEventSourcing.Messages;
using System;

namespace SimpleEventSourcing.EventSourcing
{
    public abstract class EventStreamItem
    {
        private List<IEvent> changes;
        protected Dictionary<Type, Action<IEvent>> eventAppliers;

        protected EventStreamItem()
        {
            changes = new List<IEvent>();
            eventAppliers = new Dictionary<Type, Action<IEvent>>();
        }

        protected void ApplyChanges(IEvent evt)
        {
            this.Apply(evt);
            this.changes.Add(evt);
        }

        public abstract string Name { get; }

        private void Apply(IEvent evt)
        {
            if(!eventAppliers.ContainsKey(evt.GetType()))
            {
                throw new EventSourceException("No Event Applier Registered For " + evt.GetType());
            }
            eventAppliers[evt.GetType()](evt);
        }

        public void LoadFromHistory(IEnumerable<IEvent> history)
        {
            foreach(var evt in history)
            {
                Apply(evt);
            }
        }

        public IEnumerable<IEvent> GetUncommitedChanges()
        {
            return this.changes.AsReadOnly();
        }

        public void Commit()
        {
            this.changes.Clear();
        }
    }
}