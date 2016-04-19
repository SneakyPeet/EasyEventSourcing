using System;

namespace EasyEventSourcing.EventSourcing.Exceptions
{
    [Serializable]
    public class NoCommandHandlerRegisteredException : EventSourceException
    {
        public NoCommandHandlerRegisteredException(Type command) 
            : base (string.Format("No command Handler Registered for {0}", command.Name))
        {
        }
    }
}