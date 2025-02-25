using Afisha.Application.DTO;

namespace Afisha.Application.Services.Interfaces;

public interface IEventService
{
    /// <summary>
    ///     Получение информации о событии по идентификатору
    /// </summary>
    Task<OutputEvent> GetEventyByIdAsync(long id, CancellationToken cancellationToken);

    /// <summary>
    ///     Создание нового события
    /// </summary>
    Task CreateEvent(CreateEvent createEvent, CancellationToken cancellationToken);

}
