using MassTransit;
using Microsoft.AspNetCore.Mvc;
using MTRequestFilterBug;

var builder = WebApplication.CreateBuilder(args);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer().AddSwaggerGen();

builder.Services.AddMassTransit(config =>
{
    config.UsingInMemory((context, cfg) =>
    {
        cfg.ConfigureEndpoints(context);
        cfg.UseConsumeFilter(typeof(SlicePizzaFilter<>), context);
    });
    config.AddConsumer<DeliverPizzaConsumer>();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

async Task<object> DeliverPizza([FromServices] IRequestClient<DeliverPizza> pizzaDeliveryClient, 
    [FromQuery] string pizzaName = "Marguerita")
{
    var request = new DeliverPizza(pizzaName, "477 Pizza Street, Weird City");
    var response = await pizzaDeliveryClient.GetResponse<PizzaDelivered>(request);

    if (response.Message.IsPizzaSliced) // Then the filter works!
    {
        return new
        {
            ClientReview = "The pizza was really good and sliced correctly. Amazing, would recommend.",
            Rating = "20/20"
        };
    }
    else
    {
        return new
        {
            ClientReview = "The pizza was really good but EXCUSE ME WHY ISN'T IT PRE SLICED???",
            Rating = "14/20",
            WhatWentWrong = "The SlicePizzaFilter wasn't called when receiving a response using IRequestClient."
        };
    }
}

app.MapPost("/deliver-pizza", DeliverPizza).WithName("DeliverPizza");
app.Run();