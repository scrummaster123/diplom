using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;

Console.WriteLine("Hello, World!");

var environment = Environment.GetEnvironmentVariable("WORKING_SETTINGS") ;
Console.WriteLine($"Environment: {environment}");
using var host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((hostingContext, config) =>
    {
        config.SetBasePath(Directory.GetCurrentDirectory());
        config.AddJsonFile($"appsettings.{environment}.json", optional: false, reloadOnChange: true);
    })
    .ConfigureServices((hostContext, services) =>
    {
        // Получение данных для подключения к RabbitMQ
        var config = hostContext.Configuration.GetSection("RabbitMQ");
        var rabbitHost = config["Host"];
        var port = ushort.TryParse(config["Port"], out var p) ? p : (ushort)5672;
        var user = config["User"];
        var password = config["Password"];
        var vhost = config["VirtualHost"];

        services.AddMassTransit(x =>
        {
            x.AddConsumer<NotificationConsumer>();
            // Настройка RabbitMQ
            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(rabbitHost, port, vhost, h =>
                {
                    h.Username(user);
                    h.Password(password);
                });

                // Регистрация Consumer
                cfg.ReceiveEndpoint("email_message", e =>
                {
                    e.ConfigureConsumer<NotificationConsumer>(context);
                });
            });
        });
    })
    .UseSerilog((host, log) =>
    {
        if (host.HostingEnvironment.IsProduction())
            log.MinimumLevel.Information();
        else
            log.MinimumLevel.Debug();

        log.WriteTo.Console();
    })
    .Build();



await host.RunAsync();
