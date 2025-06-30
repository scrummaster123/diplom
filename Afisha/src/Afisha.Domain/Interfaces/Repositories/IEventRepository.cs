using Afisha.Domain.Entities;

namespace Afisha.Domain.Interfaces.Repositories;

public interface IEventRepository
{
    /// <summary>
    ///     Получение списка мероприятий по фильтрам
    /// </summary>
    /// <param name="dateStart">Дата, после которой событие начинается</param>
    /// <param name="dateEnd">Дата, до которой событие начинается</param>
    /// <param name="cancellationToken"></param>
    /// <param name="locationId">Идентификатор локации, где проводится мероприятие</param>
    /// <param name="sponsorId">Идентификатор организатора</param>
    /// <returns></returns>
    Task<List<Event>> GetEventsByFiltersAsync(DateOnly dateStart, DateOnly dateEnd, CancellationToken cancellationToken,
        long? locationId = null, long? sponsorId = null);
}