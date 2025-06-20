using Afisha.Domain.Entities;

namespace Afisha.Application.DTO.Outputs;

public class OutputEventBase // ----------- (?) Название ивента ----------
{
    public long Id { get; set; }

    public String Name { get; set; }
    /// <summary>
    /// Владелец мероприятия
    /// </summary>
    public String Organizer{ get; set; }

    /// <summary>
    /// Место проведения мероприятия
    /// </summary>
    public String Location { get; set; }

    /// <summary>
    /// Дата начала мероприятия
    /// </summary>
    public DateOnly Date { get; set; }

    public long[] Participants { get; set; }
}
