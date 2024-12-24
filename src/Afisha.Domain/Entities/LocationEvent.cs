using Afisha.Domain.Entities.Abstractions;

namespace Afisha.Domain.Entities;
public class LocationEvent : EntityBase<long>
{
    public long LocationId { get; set; }

    public Location Location { get; set; }

    public long EventId { get; set; }

    public Event Event { get; set; }
}