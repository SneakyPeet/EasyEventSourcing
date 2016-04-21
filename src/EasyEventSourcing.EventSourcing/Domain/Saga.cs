using System.Collections.Generic;
using EasyEventSourcing.Messages;

namespace EasyEventSourcing.EventSourcing.Domain
{
    public abstract class Saga : EventStream
    {
        private readonly List<ICommand> unpublishedCommands;

        protected Saga()
        {
            this.unpublishedCommands = new List<ICommand>();
        }

        protected void Publish(ICommand command)
        {
            this.unpublishedCommands.Add(command);
        }
    }
}
