using System;
using System.Collections.Generic;

namespace SimpleEventSourcing.EventSourcing
{
    public interface IRepository
    {
        TAggregate GetById<TAggregate>(Guid id) where TAggregate : Aggregate;
        void Save(IEnumerable<Aggregate> aggregates);
    }
}