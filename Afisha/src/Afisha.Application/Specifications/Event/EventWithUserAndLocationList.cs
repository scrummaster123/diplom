using Microsoft.EntityFrameworkCore;

namespace Afisha.Application.Specifications.Event
{
    public class EventWithUserAndLocationList : SpecificationBase<Domain.Entities.Event>
    {
        public EventWithUserAndLocationList() : base(null,
            myevent => myevent.Include(e => e.Location)
                .Include(e => e.EventParticipants)
                    .ThenInclude(p => p.User)
            )
        {}
    }
}
