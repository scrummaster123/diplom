using Afisha.Application.Specifications.Event;
using Afisha.Domain.Entities;
using Afisha.Domain.Enums;
using Afisha.Domain.Interfaces.Repositories;
using MassTransit;
using RabbitMQModels;
using Event = Afisha.Domain.Entities.Event;

namespace Afisha.Application.Services.Managers.EventRegistration;

public class EventRegistrationBaseRule(IEventRepository eventsRepository,
    IRepository<Event, long> eventRepository,
    IUserRepository userRepository, IPublishEndpoint rabbitService) : IEventRegistrationRule
{
    
    public async Task<Message> CheckRuleAsync(long eventId, long userId, CancellationToken cancellationToken)
    {
        
        var @event = await eventRepository.GetByIdOrThrowAsync(eventId, new EventWithUserAndLocation(),
            cancellationToken: cancellationToken);

        var user = await userRepository.GetUserByIdAsync(userId, cancellationToken);

        if (user is null)
        {
            return new Message
            {
                Reason = $"Пользователь с идентификатором {userId} не найден. ",
                IsAllowed = false
            };
        }
        
        if (@event.EventParticipants.Any(x => x.UserId == user.Id))
        {
            return new Message
            {
                Reason = $"Пользователь уже зарегистрирован на мероприятие {@event}",
                IsAllowed = false
            };
        }
        
        // Проверка на наличие организатора
        if (!@event.IsOpenToRegister)
        {
            var email = @event.EventParticipants.First(x => 
                x.UserRole is EventRole.Organizer or EventRole.Manager).User.Email;
            if (email == null)
            {
                throw new Exception("Не удалось зарегистрироваться на мероприятие");
            }
            var emailMessage = new EmailMessage
            {
                Content = $"Пользователь {user.Login} отправляет заявку на участие в мероприятии.",
                Email = email
            };

            await rabbitService.Publish(emailMessage, cancellationToken);

            return new Message
            {
                Reason = "Организатору отправлена заявка на участие в мероприятии",
                IsAllowed = false,
                ApproveAdvice = "Вы можете написать организатору для участия"
            };
        }
 
        return new Message
        {
            IsAllowed = true,
        };
    }
}