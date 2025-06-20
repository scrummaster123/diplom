using Afisha.Domain.Entities;

namespace Afisha.Domain.Interfaces.Repositories;

public interface ILocationRepository
{
    /// <summary>
    ///     Получение списка локаций
    /// </summary>
    /// <returns></returns>
    Task<List<Location>> GetLocationsAsync(CancellationToken cancellationToken);
}