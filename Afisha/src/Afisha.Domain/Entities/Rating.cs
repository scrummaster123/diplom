using Afisha.Domain.Entities.Abstractions;
using System.ComponentModel.DataAnnotations;

namespace Afisha.Domain.Entities;

/// <summary>
/// Оценка\отзыв пользователя о мероприятии
/// </summary>
public class Rating : EntityBase<long>
{
    /// <summary>
    /// Значение - от 1 до 5
    /// </summary>
    [Range(1, 5, ErrorMessage = "The value must be between 1 and 5")]
    public int Value { get; set; }

    /// Текст отзыва
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Мероприятие, на которое оставлен отзыв
    /// </summary>    
    public required Event Event { get; set; }

    /// <summary>
    ///  Пользователь, оставивший отзыв
    /// </summary>
    public required User User { get; set; }
}