using Afisha.Domain.Entities;

namespace Afisha.Application.DTO;

public class OutputEvent
{
    /// <summary>
    /// Владелец мероприятия
    /// </summary>
    public OutputEventUser Sponsor { get; set; }

    /// <summary>
    /// Место проведения мероприятия
    /// </summary>
    public OutputEventLocation Location { get; set; }

    /// <summary>
    /// Дата начала мероприятия
    /// </summary>
    public DateOnly DateStart { get; set; }
}
