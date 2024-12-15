using Afisha.Domain.Entities;

namespace Afisha.Domain.Contracts;

public interface ILocationService
{
        Task<Location> GetLocationByIdAsync(long id, CancellationToken cancellationToken);
}