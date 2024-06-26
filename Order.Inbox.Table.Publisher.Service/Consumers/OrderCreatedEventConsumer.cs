using MassTransit;
using Microsoft.EntityFrameworkCore;
using Order.Inbox.Table.Publisher.Service.Entities;
using Shared.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Order.Inbox.Table.Publisher.Service.Consumers
{
    public class OrderCreatedEventConsumer(OrderInboxDbContext dbContext) : IConsumer<OrderCreatedEvent>
    {
        public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
        {
            OrderInbox orderInbox = new()
            {
                Processed = false,
                Payload = JsonSerializer.Serialize(context.Message)
            };

            await dbContext.OrderInboxes.AddAsync(orderInbox);
            await dbContext.SaveChangesAsync();

            List<OrderInbox> orderInboxes = await dbContext.OrderInboxes
                .Where(x => x.Processed == false)
                .ToListAsync();

            //event
            InboxToStockEvent inboxToStockEvent = new()
            {
                OrderInboxes = orderInboxes.Select(x => new Shared.Messages.OrderInboxMessage
                {
                    Id = x.Id,
                    Payload = x.Payload,
                    Processed = x.Processed
                }).ToList(),
            };

            


        }
    }
}
