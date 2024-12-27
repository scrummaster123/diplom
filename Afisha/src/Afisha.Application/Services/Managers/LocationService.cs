using Afisha.Application.Services.Interfaces;
using Afisha.Domain.Entities;
using Afisha.Domain.Interfaces;
using Afisha.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Afisha.Application.Services.Managers;

public class LocationService(
    IRepository<Location, long> locationRepository,
    IRepository<User, long> userRepository,
    IUnitOfWork unitOfWork) : ILocationService
{
    /// <summary>
    ///     Получение общей информации о локации по идентификатору
    /// </summary>
    public async Task<Location> GetLocationByIdAsync(long id, CancellationToken cancellationToken)
    {
        var location = await locationRepository.GetByIdAsync(
            id,
            include: q => q.Include(l => l.Owner),
            cancellationToken: cancellationToken);

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
        var owner = await userRepository.GetByIdOrThrowAsync(ownerId, cancellationToken: cancellationToken);

        // Присвоение владельца локации
        location.Owner = owner;
        var addedLocation = locationRepository.Add(location);

        // Если SaveChanges отработал, то возвращается добавленная локация
        var affectedRows = await unitOfWork.CommitAsync(cancellationToken);
        if (affectedRows > 0)
            return addedLocation;

        throw new Exception("Не удалось добавить локацию");
    }
}