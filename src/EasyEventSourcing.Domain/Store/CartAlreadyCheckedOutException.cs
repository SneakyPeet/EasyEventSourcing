using System;
using EasyEventSourcing.EventSourcing.Domain;

namespace EasyEventSourcing.Domain.Store
{
    [Serializable]
    public class CartAlreadyCheckedOutException : DomainException
    {
    }
}