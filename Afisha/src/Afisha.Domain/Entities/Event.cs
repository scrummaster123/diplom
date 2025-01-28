using Afisha.Domain.Entities.Abstractions;

namespace Afisha.Domain.Entities;

public class Event : EntityBase<long>
{
    /// <summary>
    /// Id пользователя - владельца мероприятия
    /// </summary>
    public long SponsorId { get; set; }

    /// <summary>
    /// Владелец мероприятия
    /// </summary>
    public User Sponsor { get; set; }

    /// <summary>
    /// Id места проведения мероприятия
    /// </summary>
    public long LocationId { get; set; }

    /// <summary>
    /// Место проведения мероприятия
    /// </summary>
    public Location Location { get; set; }

    /// <summary>
    /// Дата начала мероприятия
    /// </summary>
    public DateOnly DateStart { get; set; }
   
}