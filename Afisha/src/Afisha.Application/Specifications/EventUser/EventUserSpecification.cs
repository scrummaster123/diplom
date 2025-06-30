namespace Afisha.Application.Specifications.EventUser
{
    public class EventUserSpecification : SpecificationBase<Domain.Entities.EventUser>
    {
        public EventUserSpecification(long eventId, long userId) : base(
                eventUser => eventUser.EventId == eventId && eventUser.UserId == userId, null, null) { }  
    }
}
