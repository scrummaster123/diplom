using Afisha.Application.Contracts.Repositories;
using Afisha.Domain.Contracts;
using Afisha.Domain.Entities;

namespace Afisha.Application.Services;

public class LocationService(ILocationRepository locationRepository) : ILocationService
{
    /// <summary>
    ///     Получение общей информации о локации по идентификатору
    /// </summary>
    public async Task<Location> GetLocationByIdAsync(long id, CancellationToken cancellationToken)
    {
        var location = await locationRepository.GetLocationByIdAsync(id, cancellationToken);

        if (location == null)
            throw new Exception($"Локация с идентификатором {id} не найдена");
        return location;
    }

    /// <summary>
    ///     Создание новой локации
    /// </summary>
    public async Task<Location> CreateLocation(Location location, long ownerId, CancellationToken cancellationToken)
    {
        // TODO Метод ожидает изменений с приходом отдельных моделей и создания токенов для дальнейшего получения владельца
        var addedLocation = await locationRepository.CreateLocation(location, ownerId, cancellationToken);

        if (addedLocation is not null)
            return addedLocation;

        throw new Exception("Не удалось добавить локацию");
    }
}