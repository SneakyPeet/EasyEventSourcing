namespace EasyEventSourcing.Messages.Shipping

open System
open EasyEventSourcing.Messages

type ShippingItem = {
    ItemId: Guid;
}

type StartedShippingProcess = {
    OrderId: Guid;
} with interface IEvent

type PaymentConfirmed = {
    OrderId: Guid;
} with interface IEvent

type AddressConfirmed = {
    OrderId: Guid;
} with interface IEvent

type OrderDelivered = {
    OrderId: Guid;
} with interface IEvent