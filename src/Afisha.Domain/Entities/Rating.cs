namespace Afisha.Domain.Entities;

/// <summary>
/// Отзыв о мероприятии
/// </summary>
public class Rating : EntityBase<long>
{
    /// <summary>
    /// Оценка 
    /// </summary>
    public int Value { get; set; }
    //  modelBuilder.Entity<Rating>().ToTable(t => t.HasCheckConstraint("Value", "Age > 1 AND Age < 5"));

    /// <summary>
    /// Дата создания
    /// </summary>
    public DateTime CreateDate { get; set; }  = DateTime.Now;
    /// <summary>
    /// Текст отзыва
    /// </summary>
    public string? Description { get; set; }
    /// <summary>
    /// Пользователь, оставивший отзыв
    /// </summary>
    public User User { get; set; }
}
