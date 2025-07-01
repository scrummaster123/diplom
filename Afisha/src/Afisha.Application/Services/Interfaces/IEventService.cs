using Afisha.Application.DTO.Inputs;
using Afisha.Application.DTO.Outputs;
using Afisha.Application.Enum;
using Afisha.Application.Services.Managers.EventRegistration;

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
    
    /// <summary>
    ///     Регистрация на мероприятие
    /// </summary>
    /// <param name="eventId">Идентификатор мероприятия</param>
    /// <param name="userId">ID пользователя, который хочет зарегистрироваться</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>True - удалось зарегистрироваться. False - регистрация не удалась</returns>
    Task<Message> RegisterToEventAsync(long eventId, long userId, CancellationToken cancellationToken);
}
