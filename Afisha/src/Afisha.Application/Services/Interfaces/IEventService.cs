using Afisha.Application.DTO.Inputs;
using Afisha.Application.DTO.Outputs;
using Afisha.Application.Enum;

namespace Afisha.Application.Services.Interfaces;

public interface IEventService
{
    /// <summary>
    ///     Получение информации о событии по идентификатору
    /// </summary>
    Task<OutputEvent> GetEventByIdAsync(long id, CancellationToken cancellationToken);

    /// <summary>
    ///     Создание нового события
    /// </summary>
    Task CreateEvent(CreateEvent createEvent, CancellationToken cancellationToken);
    
    /// <summary>
    ///     Получение списка мероприятий по фильтрам
    /// </summary>
    Task<List<OutputEvent>> GetEventsByFilterAsync(DateOnly? dateStart, DateOnly? dateEnd,
        CancellationToken cancellationToken, long? locationId = null, long? sponsorId = null, OrderByEnum orderByEnum = OrderByEnum.Default);


    Task<IEnumerable<OutputEventBase>> GetEventsPagedAsync(int page, int pageSize, CancellationToken cancellationToken);

    Task<int> GetTotalEventsCountAsync(CancellationToken cancellationToken);
}
