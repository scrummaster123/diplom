using Afisha.Domain.Entities;

namespace Afisha.Domain.Contracts;

public interface ILocationService
{
        /// <summary>
        ///     Получение общей информации о локации по идентификатору
        /// </summary>
        Task<Location> GetLocationByIdAsync(long id, CancellationToken cancellationToken);
        
        /// <summary>
        ///     Создание новой локации 
        /// </summary>
        Task<Location> CreateLocation(Location location,long ownerId, CancellationToken cancellationToken);
}