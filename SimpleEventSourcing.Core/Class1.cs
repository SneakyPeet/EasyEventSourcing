using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleEventSourcing.Core
{
    public interface ICommand { }

    public interface IEvent { }

    public interface ICommandHandler<TCommand> where TCommand : ICommand
    {
        void Handle(TCommand command);
    }

    public interface IApplication
    {
        void Handle<TCommand>(TCommand command) where TCommand : ICommand;
    }

    public interface ICommandHandlerFactory
    {
        ICommandHandler<TCommand> Resolve<TCommand>() where TCommand : ICommand;
    }

    public class Application : IApplication
    {
        private readonly ICommandHandlerFactory factory;

        public Application(ICommandHandlerFactory factory)
        {
            this.factory = factory;
        }
        public void Handle<TCommand>(TCommand command) where TCommand : ICommand
        {
            var handler = factory.Resolve<TCommand>();
            handler.Handle(command);
        }
    }

    public interface IEventStore
    {
        TAggregate GetById<TAggregate>(Guid id) where TAggregate : Aggregate;
        void Save(IEnumerable<Aggregate> aggregates);
    }

    public abstract class Aggregate
    {

    }
}
