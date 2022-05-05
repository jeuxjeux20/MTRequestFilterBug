using MassTransit;

namespace MTRequestFilterBug;

public class SlicePizzaFilter<T> : IFilter<ConsumeContext<T>> where T : class
{
    private readonly ILogger<SlicePizzaFilter<T>> _logger;

    public SlicePizzaFilter(ILogger<SlicePizzaFilter<T>> logger)
    {
        _logger = logger;
    }

    public async Task Send(ConsumeContext<T> context, IPipe<ConsumeContext<T>> next)
    {
        _logger.LogDebug("SlicePizzaFilter running with message of type {Type}", context.Message.GetType());
        if (context.Message is PizzaDelivered { IsPizzaSliced: false } pizzaDelivered)
        {
            _logger.LogInformation("Cutting pizza after receiving it...");

            await Task.Delay(250); // They're quick at cutting pizzas
            pizzaDelivered.IsPizzaSliced = true;
        }

        await next.Send(context);
    }

    public void Probe(ProbeContext context)
    {
    }
}