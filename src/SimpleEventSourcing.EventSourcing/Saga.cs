using System;
using System.Collections.Generic;
using SimpleEventSourcing.Messages;

namespace SimpleEventSourcing.EventSourcing
{
    public abstract class Saga : EventsAsHistory
    {
        private List<ICommand> unpublishedCommands;

        protected Saga()
        {
            unpublishedCommands = new List<ICommand>();
        }

        protected void Publish(ICommand command)
        {
            unpublishedCommands.Add(command);
        }
    }
}
