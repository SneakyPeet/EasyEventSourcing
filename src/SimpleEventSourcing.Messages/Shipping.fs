namespace SimpleEventSourcing.Messages.Shipping

open System
open SimpleEventSourcing.Messages

type ShippingItem = {
    ItemId: Guid;
}

type ShipOrder = {
    ClientId: Guid;
    OrderId: Guid;
    Items: ShippingItem[];
} with interface ICommand

type OrderShipped = {
    ClientId: Guid;
    OrderId: Guid;
    Items: ShippingItem[];
} with interface IEvent