namespace Afisha.Application.DTO.Outputs;

public class OutputEventLocation
{
    /// <summary>
    ///     Наименование площадки
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    ///     Стоимость для проведения. Default = 0. Проведение бесплатное
    /// </summary>
    public decimal Pricing { get; set; }

    /// <summary>
    ///     Является ли площадка для проведения помещением (кафе, дом, актовый зал), или нет (заповедник, лес, просто открытая
    ///     зона)
    /// </summary>
    public bool IsWarmPlace { get; set; }
}
