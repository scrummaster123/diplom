using System.ComponentModel.DataAnnotations;
using Afisha.Domain.Entities.Abstractions;

namespace Afisha.Domain.Entities;

public class Location : EntityBase<long>
{
    /// <summary>
    ///     Владелец площадки
    /// </summary>
    public required User Owner { get; set; }

    /// <summary>
    ///     Наименование площадки
    /// </summary>
    [MaxLength(140)]
    public required string Name { get; set; }

    /// <summary>
    ///     Стоимость для проведения. Default = 0. Проведение бесплатное
    /// </summary>
    public decimal Pricing { get; set; }

    /// <summary>
    ///     Является ли площадка для проведения помещением (кафе, дом, актовый зал), или нет (заповедник, лес, просто открытая
    ///     зона)
    /// </summary>
    public bool IsWarmPlace { get; set; }

    /// <summary>
    /// Коллекция Location-Event
    /// </summary>
    public ICollection<LocationEvent> LocationEvents { get; set; } = Array.Empty<LocationEvent>();
}