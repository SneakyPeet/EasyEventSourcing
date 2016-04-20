using EasyEventSourcing.Messages;

namespace EasyEventSourcing.Application
{
    public interface IDispatcher<in TMessage> where TMessage : IMessage
    {
        void Send(TMessage command);
    }
}