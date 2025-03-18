namespace Afisha.Application;

public class CreateLocationModel
{
    /// <summary>
    ///     Идентификатор пользователя для проведения мероприятия
    /// </summary>
    public long UserId { get; set; }
    /// <summary>
    ///     Стоимость для проведения. Default = 0. Проведение бесплатное
    /// </summary>
    public decimal Pricing { get; set; }
    /// <summary>
    ///     Наименование локации
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    ///     Является ли площадка для проведения помещением (кафе, дом, актовый зал), или нет (заповедник, лес, просто открытая
    ///     зона)
    /// </summary>
    public bool IsWarmPlace { get; set; }




}