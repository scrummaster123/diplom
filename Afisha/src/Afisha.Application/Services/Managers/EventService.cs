using Afisha.Application.Services.Interfaces;
using Afisha.Domain.Entities;
using Afisha.Domain.Interfaces.Repositories;
using Afisha.Domain.Interfaces;
using Afisha.Application.Specifications.Event;
using AutoMapper;
using Afisha.Application.Mappers;
using Afisha.Application.DTO.Inputs;
using Afisha.Application.DTO.Outputs;
using Afisha.Application.Enum;
using Afisha.Application.Services.Managers.EventRegistration;
using Afisha.Domain.Enums;
using MassTransit;
using Microsoft.Extensions.Logging;
using RabbitMQModels;
using Event = Afisha.Domain.Entities.Event;

namespace Afisha.Application.Services.Managers;

public class EventService(
    IRepository<Event, long> eventRepository,
    IUnitOfWork unitOfWork,
    AutoMapperConfiguration autoMapperConfiguration,
    IEventRepository eventsRepository,
    IUserRepository userRepository,
    IMapper mapper,
    IEventRegistrationRule eventRegistrationRule,
    ILogger<EventService> logger,
    IPublishEndpoint rabbitService) : IEventService
{
    public async Task CreateEvent(CreateEvent createEvent, CancellationToken cancellationToken)
    {
        var config = autoMapperConfiguration.Configure();
        var iMapper = config.CreateMapper();
        var eventItem = iMapper.Map<CreateEvent, Event>(createEvent);
        eventItem.EventParticipants.Add(new EventUser
        {
            Event = eventItem,
            UserId = createEvent.SponsorId,
            UserRole = Domain.Enums.EventRole.Organizer
        });
        eventRepository.Add(eventItem);
        await unitOfWork.CommitAsync(cancellationToken);
    }


    /// <summary>
    ///     Получение списка мероприятий по фильтрам
    /// </summary>
    /// <returns>Список мероприятий по фильтру</returns>
    public async Task<List<OutputEvent>> GetEventsByFilterAsync(DateOnly? dateStart, DateOnly? dateEnd,
        CancellationToken cancellationToken,
        long? locationId = null, long? sponsorId = null, OrderByEnum orderByEnum = OrderByEnum.Default)
    {
        // Проверка на наличие даты начала и конца. Выставление дефолтных значений - ближайшие 7 дней
        var startDate = dateStart ?? DateOnly.FromDateTime(DateTime.Now);
        var endDate = dateEnd ?? startDate.AddDays(7);

        // Проверка на корректность диапазона дат
        if (startDate > endDate)
            throw new ArgumentException("Start date cannot be greater than end date");

        var events =
            await eventsRepository.GetEventsByFiltersAsync(startDate, endDate, cancellationToken, locationId,
                sponsorId);
        var result = mapper.Map<List<OutputEvent>>(events);

        result = OrderBy(result, orderByEnum);

        return result;
    }

    public async Task<Message> RegisterToEventAsync(long eventId, long userId, CancellationToken cancellationToken)
    {
        var checkRegistrationResult = await eventRegistrationRule.CheckRuleAsync(eventId, userId, cancellationToken);

        var @event = await eventRepository.GetByIdOrThrowAsync(eventId, new EventWithUserAndLocation(),
            cancellationToken: cancellationToken);

        if (checkRegistrationResult.RequestStatus == RegistrationStatusEnum.OpenRegistration ||
            checkRegistrationResult.RequestStatus == RegistrationStatusEnum.ClosedRegistration)
        {
            var user = await userRepository.GetUserByIdAsync(userId, cancellationToken);
            
            if (user is null)
                throw new Exception($"Пользователь с идентификатором {userId} не найден");
            
            if (!@event.IsOpenToRegister)
            {
                var emailMessage = new EmailMessage
                {
                    Content = $"Пользователь {user.Login} отправляет заявку на участие в мероприятии.",
                    Email = user.Email
                };
                await rabbitService.Publish(emailMessage, cancellationToken);
            }

            // Добавление пользователя в мероприятие
            @event.EventParticipants.Add(new EventUser
            {
                EventId = eventId,
                UserId = user.Id,
                UserRole = EventRole.Guest,
                IsApproved = @event.IsOpenToRegister
            });
            await unitOfWork.CommitAsync(cancellationToken);
        }

        return checkRegistrationResult;
    }

    public async Task<OutputEvent> GetEventByIdAsync(long id, CancellationToken cancellationToken)
    {
        var config = autoMapperConfiguration.Configure();
        var iMapper = config.CreateMapper();
        var eventItem = await eventRepository.GetByIdOrThrowAsync(id, new EventWithUserAndLocation(),
            Domain.Enums.TrackingType.NoTracking, cancellationToken);

        var @event = await eventsRepository.GetEventByIdAsync(id, cancellationToken);

        if (@event is not null && @event.IsOpenToRegister)
        {
            var participants = await eventsRepository.GetEventParticipantsAsync(id, cancellationToken);
            @event.EventParticipants.ToList().AddRange(participants);
        }

        var outputEvent = iMapper.Map<Event, OutputEvent>(eventItem);
        var sponsor = await eventsRepository.GetSponsorByEventIdAsync(id, cancellationToken);
        if (sponsor is not null)
            outputEvent.Sponsor = iMapper.Map<User, OutputEventUser>(sponsor);
        return outputEvent;
    }

    private List<OutputEvent> OrderBy(List<OutputEvent> events, OrderByEnum orderByEnum)
    {
        return orderByEnum switch
        {
            OrderByEnum.Default => events.OrderBy(x => x.DateStart).ToList(),
            OrderByEnum.Date => events.OrderBy(x => x.DateStart).ToList(),
            OrderByEnum.Price => events.OrderByDescending(x => x.Location.Pricing).ToList(),
            OrderByEnum.Sponsor => events.OrderBy(x => x.Sponsor.Email).ToList(),
            _ => events
        };
    }
}