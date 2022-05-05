using MassTransit;

namespace MTRequestFilterBug;

public record DeliverPizza(string PizzaName, string Address);

public record PizzaDelivered
{
    public int PizzaFreshness { get; set; }
    public bool IsPizzaSliced { get; set; }
}

public class DeliverPizzaConsumer : IConsumer<DeliverPizza>
{
    private readonly ILogger<DeliverPizzaConsumer> _logger;

    public DeliverPizzaConsumer(ILogger<DeliverPizzaConsumer> logger)
    {
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<DeliverPizza> context)
    {
        var (pizzaName, address) = context.Message;
        
        _logger.LogInformation("Delivering a (not cut) {Kind} pizza to {Address}", pizzaName, address);

        await context.RespondAsync(new PizzaDelivered
        {
            PizzaFreshness = 20,
            IsPizzaSliced = false // They don't have pizza cutters... :(
        });
    }
}