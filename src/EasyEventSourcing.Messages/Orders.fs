namespace EasyEventSourcing.Messages.Orders

open System
open EasyEventSourcing.Messages

type OrderItem = {
    ProductId: Guid;
    Price: Decimal;
}

type CreateOrder = {
    OrderId: Guid;
    ClientId: Guid;
    Items: OrderItem[];
} with interface ICommand

type OrderCreated = {
    OrderId: Guid;
    ClientId: Guid;
    Items: OrderItem[];
} with interface IEvent

type PayForOrder = {
    OrderId: Guid;
} with interface ICommand

type PaymentReceived = {
    OrderId: Guid;
} with interface IEvent

type ConfirmShippingAddress = {
    OrderId: Guid;
    Address: string; //Because I am Lazy
} with interface ICommand

type ShippingAddressConfirmed = {
    OrderId: Guid;
    Address: string; 
} with interface IEvent