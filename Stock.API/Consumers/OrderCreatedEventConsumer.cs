using MassTransit;
using Microsoft.EntityFrameworkCore;
using Shared.Events;
using Stock.API.Entities;
using System.Text.Json;

namespace Stock.API.Consumers
{
    public class OrderCreatedEventConsumer(StockDbContext dbContext) : IConsumer<OrderCreatedEvent>
    {
        public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
        {
            OrderInbox orderInbox = new()
            {
                Id = Guid.NewGuid(),
                Processed = false,
                Payload = JsonSerializer.Serialize(context.Message)
            };

            await dbContext.AddAsync(orderInbox);
            await dbContext.SaveChangesAsync();

            List<OrderInbox> orderInboxes = await dbContext.OrderInboxes
                .Where(x => x.Processed == false)
                .ToListAsync();

            foreach (var _orderInbox in orderInboxes)
            {
                OrderCreatedEvent orderCreatedEvent = JsonSerializer.Deserialize<OrderCreatedEvent>(_orderInbox.Payload);

                //STOCK STUFF
                _orderInbox.Processed = true;
                await dbContext.SaveChangesAsync();
            }

        }
    }
}
