using Afisha.Domain.Entities.Abstractions;

namespace Afisha.Domain.Entities;

public class Event : EntityBase<long>
{
    /// <summary>
    /// Владелец мероприятия
    /// </summary>
    public long SponsorId { get; set; }

    /// <summary>
    /// Место проведения мероприятия
    /// </summary>
    public long LocationId { get; set; }

    /// <summary>
    /// Ссылка на пользователя - владельца мероприятия
    /// </summary>
    public User Sponsor { get; set; }

    /// <summary>
    /// Дата начала мероприятия
    /// </summary>
    public DateOnly DateStart { get; set; }

    /// <summary>
    /// Коллекция Location-Event
    /// </summary>
    public ICollection<LocationEvent> LocationEvents { get; set; } = Array.Empty<LocationEvent>();
}