The purpose of SimpleEventSourcing is to showcase a working Application with implemented business rules, built using an event sourcing architecture. The purpose of this project was to assist myself in understanding how to go about building something using event sourcing and secondly to give others new to the concept a starting point.

* The example tries to be as straightforward as possible, keeping the boilerplate code as simple as possible, while still keeping it usable and maintainable. 
* Explicit implementation is chosen over Magic. This make it clear what is going on in the system. This also forces us to feel the hurt when making bad decisions (see [talk](http://www.infoq.com/presentations/8-lines-code-refactoring) by Greg Young). 
* No external libraries are used other than for testing purposes
* Currently only an in-memory event store is implemented. I hope to add an [EventStore](https://geteventstore.com/) implementation soon.

##A Simple Overview of Event Sourcing

Event Sourcing is Command Query Responsibility Separation (CQRS), with the added benefit of no data loss. 

**Command**

We store the history of things that happened instead of state. We store this history as events. To change our system, we take these events, replay them to build up state, apply some command to this state to generate new events. `Events + Command = Event`.

**Query**

We take events, replay them to build up state in appropriate read models that are optimized for our queries.

##A Detailed Overview of the System

Here is the typical flow through the application.

**Command**

1. A Consumer of the application Creates a Command. Commands are specified in `SimpleEventSourcing.Messages`.
2. The Consumer asks application to handle the command by calling the `send` method on the `CommandDispatcher`.
3. The CommandDispatcher asks the `CommandHandlerFactory` for a `CommandHandler` that can handle the provided Command. CommandHandlers are analogous to Application Services in Domain Driven Design.
4. The Command is given to the relevant `Handle` method on the CommandHandler.
5. The CommandHandler either creates a new `Aggregate` or loads one from the `Repository` by Id.
	* When loading an Aggregate, the Repository will get all the relevant `Events` from the `EventStore`, create a fresh Aggregate and build up the Aggregate state by replaying the events using the `LoadFromHistory` method on the Aggregate.
6. The CommandHandler will call the relevant method on the Aggregate to execute the business logic. The Aggregate will then create the relevant `Events` that need to be persisted.
7. The CommandHandler then asks the Repository to save the Aggregate. The Repository asks the Aggregate for the newly created Events and provides them to the EventStore to be persisted.

This concluded writing changes into the app. This entire process is a model of `Events + Command = Events`.

**Query**

Once events have been persisted to the EventStore, they can be processed by `EventHandlers`. Each Event can be processed by 0 to Many EventHandlers. EventHandlers are responsible for 

1. Building Read State. We build up the Read Models for the queries we want to run against our data. These models are typically persisted in some sort of database that is optimized for the type of query we have, whether it be relational, graph, document etc. Note that we never query directly from the EventStore, because an EventStore by design is not good at queries.
2. Triggering Commands in different Bounded Contexts. 
3. Triggering some side effect for example sending an email. (This ties into 1. as we typically build up some sort of state (for example an email queue) that we can then read from to trigger some process.)

EventHandlers should be idempotent, meaning they can process the same event multiple times, reproducing the exact same result (for example if we receive the same email triggering event multiple times, we should still only send the email once).

#Implementation Overview
##Messages (Commands and Events)

At the core of any event sourcing implementation is the messages that the application can send. These messages form a contract with the outside world. There are two types of messages

* Commands - These are the actions that any consumer of the application can use to change the system. If you want to know what the application does, look at the commands. All commands inherit from `ICommand`.
* Events - These are the resulting side effects from sending commands to your system. Events are the main storage component of the application. They are the ultimate truth. If there is not an event, it never happened. They are used to rebuild write state before processing commands, build read state and trigger logic in other bounded contexts. All events inherit from `IEvent`.

Messages are implemented in `SimpleEventSourcing.Messages`.

I have chosen to implement all messages using F# records. This provides the following benefits

* Immutable by default - the constructor is implemented by default, reducing the amount of code that needs to be written.
* Structural Equality - this means event equality can be tested directly without the need to override the equals method, thus reducing the amount of code that needs to be written.

##Application
Along with the messages, the Application is what the outside world interacts with to change our system. Changes to the system are are triggered by sending a command to the Application. The following classes live in `SimpleEventSourcing.Application`.

###CommandDispatcher
The CommandDispatcher is the only entry point into our system. It receives Commands via it's `Send` method. It requires a `CommandHandlerFactory` to provide it with the matching `CommandHandler`. It then passes the Command to the CommandHandler.

###CommandHandlerFactory
The CommandHandlerFactory resolves `CommandHandlers` based on the type of the command. In this implementation I have chosen not to rely on a typical dependency injection container, like Castle Windsor, but to implement my own command handler resolver. The CommandHandlerFactory has at it's hart a dictionary with Command Type as Keys, and factory functions that build our command handlers as Values. This allows us to easily control the lifecycles of our objects. In SimpleEventSourcing we have single instance of our EventStore and everything else is short lived. We pass our EventStore instance to our CommandHandlerFactory as this allows us to easily mock out our EventStore during testing.

##Domain
###CommandHandlers
CommandHandlers can be seen as the Application Services of our domain. Their main purpose is orchestration. Based on the Command, they will get our state from the database, trigger the relevant domain logic, and persist our state. CommandHandlers all inherit from `ICommandHandler<TCommand>`.

###EventStreams (Aggregates and Sagas)
We implement the bulk of our domain using Aggregates and Sagas. The simplest way to describe the difference is

* Aggregates generate Events from Commands. Mainly used to implement logic inside a bounded context.
* Sagas generate Commands from Events (Can be seen as a kind of "transaction" spanning Bounded Contexts).

Both aggregates and sagas inherit from the abstract `EventStream` class. The EventStream is responsible for building state and keeping track of changes to this state in the form of events. There are two basic scenarios for building state.

**State from Command:** When an aggregate is asked to perform some action it will

1. Validate it's inputs
2. If inputs are valid it will create an Event associated with the state change
3. The Event is passed to the ApplyChanges method. This method adds the event to the list of changes then calls the Apply method.
4. The apply method resolves the relevant applier method for the event type and passes the event on
5. The state of the aggregate is updated in the relevant applier method

**State from History:** When an aggregate state is built up from history 

1. A list of events is provided to the LoadFromHistory Method
2. These events are sent to the Apply method one by one
3. The apply method resolves the relevant applier method for the event type and passes the event on
4. The state of the aggregate is updated in the relevant applier method 

NOTE:

* Applier Methods are explicitly registered in SimpleEventSourcing. We do this to get rid of magic. There are many other ways to do this by convention etc. but the goal here is to be clear. 
* Events with no applier method will throw an exception. Again this forces us to be explicit.
* Applier Methods should never throw exceptions. We should always be able to build up state from previous events, even if that state is no longer valid. Exceptions relating to logic should be thrown inside the calling method before ApplyChanges is called.

###StreamIdentifier
We identify our streams by using the aggregate/saga name as well as the relevant id (Example: `ShoppingCart-809b71b5-1fc5-4039-b7fe-5d23aa58c5b4`). 

##Persistence
###Repository
Our repository is responsible of taking a stream of events from the EventStore, creating the relevant EventStream object and replaying the events onto that stream before giving the EventStream object back to the CommandHandler. This is all achieved with the `T GetById<T>(Guid id) where T : EventStream` method.

Secondly it takes EventStream objects and passes the newly created events to the EventStore for saving. This is done using `void Save(EventStream stream)`.

I considered implementing a unit of work for persistence across multiple aggregates, but this can just as easily be modeled using `void Save(IEnumerable<EventStream> streamItems)`. Events are bundled before they are sent to the EventStore for saving. 

###EventStore
The EventStore simple persists events. Currently SimpleEventSourcing uses an in memory eventstore. [EventStore](https://geteventstore.com/) is the EventStore database of choice.

A simple store domain was chosen as everyone is familiar with it. Rules are made in such a way to provide examples of bounded context, eventual consistency and saga's.

###EventStoreStream
This is a helper class holding a stream id and the related events.

##Event Handlers

The role of event handlers are described above. See the domain implementation details for more spesific use cases.

#Domain
##Rules
###Shopping Cart

* You should be able to add and remove items from a shopping cart as well as empty a shopping cart
* When you checkout a shopping cart, an order is created

###Orders

* You should be able to pay for an order
* You should be able to confirm the delivery address for an order

###Shipping

* Once an order is and paid and address confirmed a shipping instruction should be created. For added complexity there is no sequence for paying and ordering, thus a saga is required for generating the shipping instruction.

###Customer

* If an order is shipped to a new address, the address should be added to the customer

##Implementation Details

###Shopping Cart

* When sending a command to our shopping cart, we check our read model to see if a cart exists. If a cart does not exist, we create one.
* We create a cart using the factory function on the cart. Note that the consumer provides the cart id and we are not reliant on a database to generate one for us. Internally the factory function creates a CartCreated event for us. 
* When changing the contents of our cart, we update our read model based on the events.
* When checking out, the shopping cart returns an Order aggregate. 
* Finally we remove our cart from our read model as it no longer exists.

###Orders

* Orders are created when checking out a cart.
* Orders need to be paid for and be provided with a shipping address.
* Orders are completed once the packages are shipped

###Shipping

* A shipping instruction is created when an order has been paid for and a shipping address has been provided.
* We implement this shipping logic by using a saga as a state machine. Our saga is generated from the OrderCreated Event. Once the saga has all the detail it needs, it will generate the shipping instruction as a command that we send to the relevant CommandHandler;
* We will assume a shipping instruction equates to a delivered package and mark our Order as complete.

###Customer Details

We want to add the Shipping address to the customer if it does not exist. Thinking traditionally one would want to put this on the customer Aggregate. However keeping a list of shipping addresses is mainly there to provide convenience to the customer (it's bad ux to have them fill out the same address constantly). Therefore we just build a read model containing all the customer addresses. This is a fundamental difference from traditional thinking. The aggregate state should only care about the business logic and properties not relating to the business logic should not be contained in the aggregate. However the data is not lost as we keep all history inside events. Thus we can build up read models separatly from write models. Boom CQRS. 