using Microsoft.EntityFrameworkCore;

namespace Afisha.Application.Specifications.Event
{
    public class EventWithUserAndLocation() : SpecificationIncludeBase<Domain.Entities.Event>(location => location
        .Include(e => e.EventParticipants)
        .ThenInclude(x => x.User)
        .Include(e => e.Location));
}
