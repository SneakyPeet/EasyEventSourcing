using System;

namespace EasyEventSourcing.EventSourcing
{
    public interface IRepository
    {
        T GetById<T>(Guid id) where T : EventStream, new();

        void Save(params EventStream[] streamItems);
    }
}