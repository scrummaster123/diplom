using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Afisha.NotificationService.Injections;


public static class RabbitInjection
{
    public static HostApplicationBuilder  RegisterRabbitMq(this HostApplicationBuilder  builder)
    {
        
        var rabbitHost = builder.Configuration.GetValue<string>("RabbitMQ:Host");
        ushort.TryParse(builder.Configuration.GetValue<string>("RabbitMQ:Port"), out var rabbitPort);
        var rabbitUser = builder.Configuration.GetValue<string>("RabbitMQ:User");
        var rabbitPassword = builder.Configuration.GetValue<string>("RabbitMQ:Password");
        var rabbitVirtualHost = builder.Configuration.GetValue<string>("RabbitMQ:VirtualHost");
        Console.WriteLine("RabbitMQ:Host: " + rabbitHost);
        Console.WriteLine("RabbitMQ:Port: " + rabbitPort);
        Console.WriteLine("RabbitMQ:User: " + rabbitUser);
        Console.WriteLine("RabbitMQ:Password: " + rabbitPassword);
        
        builder.Services.AddMassTransit(x =>
        {
            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(rabbitHost,port: rabbitPort, rabbitVirtualHost,"", h =>
                {
                    h.Username(rabbitUser);
                    h.Password(rabbitPassword);
                });
                cfg.ConfigureEndpoints(context);
            });
        });
        return builder;
    }
}