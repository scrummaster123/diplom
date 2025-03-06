using Microsoft.EntityFrameworkCore;

namespace Afisha.Application.Specifications.Rating
{
    internal class RatingByEventIdWithUserAndEventSpecification : SpecificationBase<Domain.Entities.Rating>
    {
        public RatingByEventIdWithUserAndEventSpecification(long eventId) : base(
                rating => rating.Event.Id == eventId,
                rating => rating.Include(r => r.User)
                .Include(r => r.Event)
            )
        { }
    }
}
