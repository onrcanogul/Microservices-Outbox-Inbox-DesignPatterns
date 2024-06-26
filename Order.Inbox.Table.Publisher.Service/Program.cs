using MassTransit;
using Microsoft.EntityFrameworkCore;
using Order.Inbox.Table.Publisher.Service;
using Order.Inbox.Table.Publisher.Service.Entities;

var builder = Host.CreateApplicationBuilder(args);



builder.Services.AddMassTransit(configure =>
{
    configure.UsingRabbitMq((context, configurator) =>
    {
        configurator.Host(builder.Configuration["RabbitMQ"]);
    });
});


builder.Services.AddDbContext<OrderInboxDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("SQLServer")));

var host = builder.Build();
host.Run();
