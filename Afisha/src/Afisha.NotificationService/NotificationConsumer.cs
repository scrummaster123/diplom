using MassTransit;
using Microsoft.Extensions.Logging;
using RabbitMQModels;

public class NotificationConsumer(ILogger<NotificationConsumer> logger) : IConsumer<EmailMessage>
{
    public async Task Consume(ConsumeContext<EmailMessage> context)
    {
        var message = context.Message;
        
        logger.LogInformation("Имитация отправки сообщения на почту пользователя с email {Email} с контеном {Content}", message.Email, message.Content);
        // рассылка на почту
    }
}