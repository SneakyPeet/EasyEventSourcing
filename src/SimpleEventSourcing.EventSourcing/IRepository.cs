using System;
using System.Collections.Generic;

namespace SimpleEventSourcing.EventSourcing
{
    public interface IRepository
    {
        T GetById<T>(Guid id) where T : EventStreamItem, new();

        void Save(EventStreamItem streamItem);

        void Save(IEnumerable<EventStreamItem> streamItems);
    }
}