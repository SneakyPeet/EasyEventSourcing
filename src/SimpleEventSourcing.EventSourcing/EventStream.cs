using System.Collections.Generic;
using SimpleEventSourcing.Messages;
using SimpleEventSourcing.EventSourcing.Exceptions;
using System;

namespace SimpleEventSourcing.EventSourcing
{
    public abstract class EventStream
    {
        private List<IEvent> changes;
        protected Dictionary<Type, Action<IEvent>> eventAppliers;

        protected EventStream()
        {
            changes = new List<IEvent>();
            eventAppliers = new Dictionary<Type, Action<IEvent>>();
            RegisterAppliers();
        }

        protected abstract void RegisterAppliers();

        protected void RegisterApplier<TEvent>(Action<TEvent> applier) where TEvent : IEvent
        {
            eventAppliers.Add(typeof(TEvent), (x) => applier((TEvent)x));
        }

        protected Guid id { get; set; }

        public string Name { get { return this.GetType().Name; } }

        protected void ApplyChanges(IEvent evt)
        {
            this.Apply(evt);
            this.changes.Add(evt);
        }

        

        public StreamIdentifier StreamIdentifier
        {
            get
            {
                return new StreamIdentifier(Name, id);
            }
        }

        private void Apply(IEvent evt)
        {
            if(!eventAppliers.ContainsKey(evt.GetType()))
            {
                throw new NoEventApplyMethodRegisteredException(evt, this);
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