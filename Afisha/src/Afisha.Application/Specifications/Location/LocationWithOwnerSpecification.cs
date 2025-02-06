
using Microsoft.EntityFrameworkCore;

namespace Afisha.Application.Specifications.Location;

public class LocationWithOwnerSpecification : SpecificationIncludeBase<Domain.Entities.Location>
{
    public LocationWithOwnerSpecification() : base(
        location => location.Include(l => l.Owner)
        )
    { }
}
