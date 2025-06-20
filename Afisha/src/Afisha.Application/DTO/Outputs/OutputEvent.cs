using Afisha.Domain.Entities;

namespace Afisha.Application.DTO.Outputs;

public class OutputEvent // ----------- (?) Название ивента ----------
{
    public long Id { get; set; }
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
    
    /// <summary>
    /// Открыто ли мероприятие для регистрации
    /// </summary>
    public bool IsOpenToRegister { get; set; }
    
    public List<OutputEventUser> Participants { get; set; } = new List<OutputEventUser>();
}
