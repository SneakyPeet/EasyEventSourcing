namespace EasyEventSourcing.Messages.Store

open System
open EasyEventSourcing.Messages

type CreateNewCart = {
    CartId: Guid;
    ClientId: Guid;
} with interface ICommand

type CartCreated = {
    CartId: Guid;
    ClientId: Guid;
} with interface IEvent

type AddProductToCart = {
    CartId: Guid;
    ProductId: Guid;
    Price: Decimal;
} with interface ICommand

type ProductAddedToCart = {
    ProductId: Guid;
    Price: Decimal;
} with interface IEvent

type RemoveProductFromCart = {
    CartId: Guid;
    ProductId: Guid;
} with interface ICommand

type ProductRemovedFromCart = {
    ProductId: Guid;
} with interface IEvent

type EmptyCart = {
    CartId: Guid;
} with interface ICommand

type CartEmptied() = interface IEvent

type Checkout = {
    CartId: Guid;
} with interface ICommand

type CartCheckedOut() = interface IEvent