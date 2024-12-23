using Afisha.Domain.Entities;

namespace Afisha.Application.Contracts.Repositories;

public interface ILocationRepository
{
    Task<Location?> GetLocationByIdAsync(long id, CancellationToken cancellationToken);
    Task<Location?> CreateLocation(Location location, long ownerId, CancellationToken cancellationToken);
}