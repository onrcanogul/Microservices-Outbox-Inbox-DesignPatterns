using MassTransit;
using Order.Outbox.Table.Publisher.Service.Entities;
using Quartz;
using Shared.Events;
using System.Text.Json;

namespace Order.Outbox.Table.Publisher.Service.Jobs
{
    public class OrderOutboxPublishJob(IPublishEndpoint publishEndpoint) : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            if (OrderOutboxSingletonDatabase.DataReaderState)
            {
                OrderOutboxSingletonDatabase.DataReaderBusy();
                List<OrderOutbox> orderOutboxes = (await OrderOutboxSingletonDatabase.QueryAsync<OrderOutbox>("SELECT * FROM OrderOutboxes WHERE ProcessedOn IS NULL ORDER BY OccuredOn ASC")).ToList();

                foreach (var orderOutbox in orderOutboxes)
                {
                    if(orderOutbox.Type == nameof(OrderCreatedEvent))
                    {
                        OrderCreatedEvent orderCreatedEvent = JsonSerializer.Deserialize<OrderCreatedEvent>(orderOutbox.Payload);

                        if (orderCreatedEvent is not null)
                        {
                            await publishEndpoint.Publish(orderCreatedEvent);
                            await OrderOutboxSingletonDatabase.ExecuteAsync($"UPDATE ORDEROUTBOXES SET ProcessedOn = GETDATE() WHERE ID = '{orderOutbox.Id}'");
                        }       
                    }
                }

                OrderOutboxSingletonDatabase.DataReaderReady();
                Console.WriteLine("Order outbox table checked!");
            }
        }
    }
}
