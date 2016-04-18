The purpose of SimpleEventSourcing is to showcase a working Application with implemented buisiness rules, built using an event sourcing architecture. 

* The example tries to be as straigtforward as possible, keeping the boilerplater code as simple as possible, while still keeping it usable and maintainable. 
* Explisit implementation is chosen over Magic. This make it clear what is going on in the system. This also forces us to feel the hurt when making bad decisions (see Greg Youngs [talk](http://www.infoq.com/presentations/8-lines-code-refactoring)). 
* No external libraries are used other than for testing purposes
* Currently only an in-memory event store is implemented. I hope to add an [EventStore](https://geteventstore.com/) implementation soon.

##Implementation Details

Below follow some information on the archtecture implementation choices.

###Overview

add fancy image here

##Project Overview
###SimpleEventSourcing.Messages

At the core of any event sourcing implementation is the messages that the application can send. These messages form a contract with the outside world. There are two types of messages

* Commands - These are the actions that any consumer of the application can use to write data (events) into the system. If you want to know what the application does, look at the commands. All commands inherit from `ICommand`.
* Events - These are the resulting side effects from sending commands to your system. Events are the main storage component of the application. They are the ultimate truth. They are used to rebuild write state before processing commands, build read state and trigger logic in other bounded contexts. All events inhertit from `IEvent`.

I have chosen to implement all messages using F# records. This provides the following benefits

* Immutable by default - the constructor is implemented by default, reducing the amount of code that needs to be written.
* Structural Equality - this means event equality can be tested directly without the need to overide the equals method, thus reducing the amount of code that needs to be written.

###SimpleEventSourcing.EventSourcing
Contains all the event sourcing 


##Business Rules

A simple store domain was chosen as everyone is familiar with it. Rules are made in such a way to provide examples of bounded context, eventual consistency and saga's.

###Shopping Cart

* You should be able to add and remove items from a shopping cart as well as empty a shopping cart
* When you checkout a shopping cart, an order is created

###Orders

* You should be able to pay for an order
* You should be able to confirm the delivery address for an order

###Shipping

* Once an order is and paid and address confirmed a shipping instruction should be created

(Because there is no sequence for paying and ordering, a saga is required for the shipping instruction)

###Customer

* If an order is shipped to a new address, the address should be added to the customer (eventual consistency + read model)

