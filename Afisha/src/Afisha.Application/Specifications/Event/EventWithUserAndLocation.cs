using Microsoft.EntityFrameworkCore;

namespace Afisha.Application.Specifications.Event
{
    public class EventWithUserAndLocation : SpecificationIncludeBase<Domain.Entities.Event>
    {
        public EventWithUserAndLocation() : base(
            location => location.Include(e => e.Location)
                .Include(e => e.EventParticipants)
            )
        {}
    }
}
