using Afisha.Application.Services.Interfaces;
using Afisha.Domain.Entities;
using Afisha.Domain.Interfaces.Repositories;
using Afisha.Domain.Interfaces;
using Afisha.Application.Specifications.Event;
using AutoMapper;
using Afisha.Application.Mappers;
using Afisha.Application.DTO.Inputs;
using Afisha.Application.DTO.Outputs;

namespace Afisha.Application.Services.Managers;

public class EventService(IRepository<Event, long> eventRepository,
    IUnitOfWork unitOfWork,
    AutoMapperConfiguration autoMapperConfiguration) : IEventService
{
    public async Task CreateEvent(CreateEvent createEvent, CancellationToken cancellationToken)
    {
        var config = autoMapperConfiguration.Configure();
        var iMapper = config.CreateMapper();
        var eventItem = iMapper.Map<CreateEvent, Event>(createEvent);
        eventRepository.Add(eventItem);
        await unitOfWork.CommitAsync(cancellationToken);
    }

    public async Task<OutputEvent> GetEventByIdAsync(long id, CancellationToken cancellationToken)
    {
        var config = autoMapperConfiguration.Configure();
        var iMapper = config.CreateMapper();
        var eventItem = await eventRepository.GetByIdOrThrowAsync(id, new EventWithUserAndLocation(), Domain.Enums.TrackingType.NoTracking, cancellationToken);
        var outputEvent = iMapper.Map<Event, OutputEvent>(eventItem);
        return outputEvent;
    }
}
