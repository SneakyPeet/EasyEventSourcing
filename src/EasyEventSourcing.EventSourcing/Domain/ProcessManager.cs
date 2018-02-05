using System.Collections.Generic;
using EasyEventSourcing.Messages;

namespace EasyEventSourcing.EventSourcing.Domain
{
    public abstract class ProcessManager : EventStream
    {
        private readonly List<ICommand> unpublishedCommands;

        protected ProcessManager()
        {
            this.unpublishedCommands = new List<ICommand>();
        }

        protected void Publish(ICommand command)
        {
            this.unpublishedCommands.Add(command);
        }
    }
}
