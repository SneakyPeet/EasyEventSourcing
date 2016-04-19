namespace EasyEventSourcing.Messages.Shipping

open System
open EasyEventSourcing.Messages

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