using System.Transactions;
using Afisha.Application.Contracts.Repositories;
using Afisha.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Afisha.Infrastructure.Data.Repositories;

public class LocationRepository(AfishaDbContext context, IServiceProvider Services) : ILocationRepository
{
    /// <summary>
    ///     Получение локации по идентификатору
    /// </summary>
    public async Task<Location?> GetLocationByIdAsync(long id, CancellationToken cancellationToken)
    {
        

        var location = await context.Locations
            .Include(x => x.Owner)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        return location;
    }

    /// <summary>
    ///     Создание новой локации
    /// </summary>
    public async Task<Location?> CreateLocation(Location location, long ownerId, CancellationToken cancellationToken)
    {
        // Поиск пользователя с целью убедиться, что он существует в базе данных
        var owner = await context.Users.FirstOrDefaultAsync(x => x.Id == ownerId, cancellationToken);
        if (owner is null)
            throw new Exception("Не удалось получить данные о владельце локации");

        // Присвоение владельца локации
        location.Owner = owner;
        var addedLocation = context.Locations.Add(location);

        // Если SaveChanges отработал, то возвращается добавленная локация
        if (await context.SaveChangesAsync(cancellationToken) > 0)
            return addedLocation.Entity;

        throw new Exception("Не удалось сохранить новую локацию");
    }
}