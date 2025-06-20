using Afisha.Domain.Entities;
using Afisha.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Afisha.Infrastructure.Data.Repositories;

public class LocationRepository(AfishaDbContext context) : ILocationRepository
{
    public Task<List<Location>> GetLocationsAsync(CancellationToken cancellationToken)
    {
        return context.Locations.ToListAsync(cancellationToken);
    }
}