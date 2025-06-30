namespace Afisha.Application.DTO.Inputs;

public class CreateEvent
{
    /// <summary>
    /// Id пользователя - владельца мероприятия
    /// </summary>
    public long SponsorId { get; set; }

    /// <summary>
    /// Id места проведения мероприятия
    /// </summary>
    public long LocationId { get; set; }

    /// <summary>
    /// Дата начала мероприятия
    /// </summary>
    public DateOnly DateStart { get; set; }
}
