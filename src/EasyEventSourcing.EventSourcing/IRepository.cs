using System;
using System.Collections.Generic;

namespace EasyEventSourcing.EventSourcing
{
    public interface IRepository
    {
        T GetById<T>(Guid id) where T : EventStream, new();

        void Save(EventStream stream);

        void Save(IEnumerable<EventStream> streamItems);
    }
}