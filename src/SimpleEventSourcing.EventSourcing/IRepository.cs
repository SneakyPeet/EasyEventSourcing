using System;
using System.Collections.Generic;

namespace SimpleEventSourcing.EventSourcing
{
    public interface IRepository
    {
        T GetById<T>(Guid id) where T : EventStream, new();

        void Save(EventStream stream);

        void Save(IEnumerable<EventStream> streamItems);
    }
}