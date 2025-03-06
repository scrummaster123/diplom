namespace Afisha.Domain.Entities.Dto;

public class RatingDto
{
    /// <summary>
    /// Значение 1 - 5
    /// </summary>
    public int Value { get; set; }
    /// <summary>
    /// Текст отзыва
    /// </summary>
    public string Description { get; set; } = string.Empty;
    /// <summary>
    /// Событие, к которому оставлен отзыв
    /// </summary>
    public long EventId { get; set; }
    /// <summary>
    /// Пользователь, оставивший отзыв
    /// </summary>
    public long UserId { get; set; }
}
