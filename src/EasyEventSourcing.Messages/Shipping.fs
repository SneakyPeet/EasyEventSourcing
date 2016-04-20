namespace EasyEventSourcing.Messages.Shipping

open System
open EasyEventSourcing.Messages

type ShippingItem = {
    ItemId: Guid;
}

type ShipOrder = {
    OrderId: Guid;
} with interface ICommand

type OrderCompleted = {
    OrderId: Guid;
} with interface IEvent