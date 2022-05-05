# MassTransit bug(?) reproduction project

This repo serves as a reproducible example of a bug (or not?) 
where the `ConsumeContext<T>` scoped filter isn't called when receiving 
a response from a request using `IRequestClient<T>`.

This has been tested with the in-memory provider and with the RabbitMQ provider.

Your goal will be to make `SlicePizzaFilter` work, so received pizzas are sliced
correctly and make the client happy. Without touching `DeliverPizzaConsumer`
because that's just cheating.

### Requirements

* .NET 6 SDK

### Where should I look at?

* `Program.cs` contains all the service registrations, and a dumb simple API to call the `IRequestClient<T>` 
using Minimal APIs. 
* `DeliverPizza.cs` contains the `DeliverPizza` request, the `PizzaDelivered`
response and the `DeliverPizzaConsumer` which processes the request.
* `SlicePizzaFilter.cs` contains the filter that is *supposed to* slice the
pizza when receiving it, it's registered as a scoped filter for `ConsumeContext<T>`.