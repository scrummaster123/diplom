using Microsoft.EntityFrameworkCore;

namespace Afisha.Application.Specifications.Location;

public class LocationWithOwnerAndEventsSpecification : SpecificationIncludeBase<Domain.Entities.Location>
{
    public LocationWithOwnerAndEventsSpecification() : base(
        location => location.Include(l => l.Owner)
            .Include(x => x.Events)
    )
    {
    }
}