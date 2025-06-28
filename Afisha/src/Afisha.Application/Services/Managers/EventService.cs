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
using Afisha.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using Afisha.Application.Specifications.EventUser;

namespace Afisha.Application.Services.Managers;

public class EventService(
    IRepository<Event, long> eventRepository,
    IRepository<EventUser, long> eventUsertRepository,
    IUnitOfWork unitOfWork,
    AutoMapperConfiguration autoMapperConfiguration,
    IEventRepository eventsRepository,
    IMapper mapper) : IEventService
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

    public async Task<OutputEvent> GetEventByIdAsync(long id, CancellationToken cancellationToken)
    {
        var config = autoMapperConfiguration.Configure();
        var iMapper = config.CreateMapper();
        var eventItem = await eventRepository.GetByIdOrThrowAsync(id, new EventWithUserAndLocation(),
            Domain.Enums.TrackingType.NoTracking, cancellationToken);
        var outputEvent = iMapper.Map<Event, OutputEvent>(eventItem);
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

    public async Task<IEnumerable<OutputEventBase>> GetEventsPagedAsync(int page, int pageSize, CancellationToken cancellationToken)
    {
        int skip = (page - 1) * pageSize;
        var events = await eventRepository.GetPagedAsync(new EventWithUserAndLocationList(), TrackingType.Tracking, page, pageSize, cancellationToken);


        return events?.Select(e => new OutputEventBase
        {
            Id = e.Id,
            Name = "<Без названия>",
            Date = e.DateStart,
            Location = e.Location.Name,
            Organizer = e.EventParticipants.Where(x=>x.UserRole.Equals(EventRole.Organizer)).Select(u => $"{u.User.FirstName} {u.User.LastName}").FirstOrDefault(),
            Participants = e.EventParticipants.Select(x => x.UserId).ToArray()
        }) ?? new List<OutputEventBase>();
    }

    public async Task<int> GetTotalEventsCountAsync(CancellationToken cancellationToken)
    {
        return await eventRepository.GetTotalCountAsync(cancellationToken);
    }

    public async Task JoinEventAsync(long eventId, long userId, CancellationToken cancellationToken)
    {
        //TODO: Добавить проверки
        var eventUser = new EventUser{
            EventId = eventId,
            UserId = userId,
            UserRole = EventRole.Guest
        };
        eventUsertRepository.Add(eventUser);
        await unitOfWork.CommitAsync(cancellationToken);
    }

    public async Task LeaveEventAsync(long eventId, long userId, CancellationToken cancellationToken)
    {
        var eventUsers = await eventUsertRepository.GetAsync(new EventUserSpecification(eventId, userId));
        var eventUser = eventUsers.FirstOrDefault();
        if (eventUser != null) {
            if (eventUser.UserRole != EventRole.Organizer)
            {
                await eventUsertRepository.DeleteAsync(eventUser.Id, cancellationToken);
                await unitOfWork.CommitAsync(cancellationToken);
            }
        }
    }
}
