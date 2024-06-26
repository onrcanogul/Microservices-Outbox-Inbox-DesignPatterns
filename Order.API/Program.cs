using Microsoft.EntityFrameworkCore;
using Order.API.Models;
using Order.API.Models.ViewModels;
using Shared.Events;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<OrderDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("SQLServer")));

var app = builder.Build();



    app.UseSwagger();
    app.UseSwaggerUI();


app.UseHttpsRedirection();

app.MapPost("/create-order", async (CreateOrderVM model, OrderDbContext context) =>
{
    Order.API.Models.Order order = new()
    {
        BuyerId = model.BuyerId,
        OrderStatus = Order.API.Models.Enums.OrderStatus.Pending,
        TotalPrice = model.OrderItems.Sum(oi => oi.Price * oi.Count),
        OrderItems = model.OrderItems.Select(oi => new OrderItem
        {
            Count = oi.Count,
            ProductId = oi.ProductId,
            Price = oi.Price,
        }).ToList()
    };
    await context.Orders.AddAsync(order);

    var idempotentToken = Guid.NewGuid();
    OrderCreatedEvent orderCreatedEvent = new()
    {
        BuyerId = order.BuyerId,
        OrderId = order.Id,
        OrderItems = order.OrderItems.Select(oi => new Shared.Messages.OrderItemMessage
        {
            Count = oi.Count,
            ProductId = oi.ProductId,
            Price = oi.Price,
        }).ToList(),
        TotalPrice = order.TotalPrice,
        IdempotentToken = idempotentToken
    };
    OrderOutbox orderOutbox = new()
    {
        OccuredOn = DateTime.UtcNow,
        ProcessedOn = null,
        Payload = JsonSerializer.Serialize(orderCreatedEvent),
        Type = orderCreatedEvent.GetType().Name,
        IdempotentToken = idempotentToken
    };
    await context.OrderOutboxes.AddAsync(orderOutbox);

    await context.SaveChangesAsync();
});





app.Run();


