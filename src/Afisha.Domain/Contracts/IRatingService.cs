using Afisha.Domain.Entities;

namespace Afisha.Domain.Contracts;

public interface IRatingService
{
    Task<Rating> GetLocationByIdAsync(long id, CancellationToken cancellationToken);
}
