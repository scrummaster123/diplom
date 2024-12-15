using Afisha.Domain.Contracts;
using Afisha.Domain.Entities;

namespace Afisha.Application.Services;

public class LocationService : ILocationService
{
    public async Task<Location> GetLocationByIdAsync(long id, CancellationToken cancellationToken)
    {
        var location = new Location { Id = id, Name = "Location", Owner = new User()};

        await Task.Delay(1, cancellationToken);
        
        return location;
    }
}