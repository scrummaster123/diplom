namespace Afisha.Application.DTO;

public class OutputEventUser
{
    /// <summary>
    /// Имя пользователя
    /// </summary>
    public string FirstName { get; set; }

    /// <summary>
    /// Фамилия пользователя
    /// </summary>
    public string LastName { get; set; }

    /// <summary>
    /// Отчество пользователя (необязательное поле)
    /// </summary>
    public string? Patronymic { get; set; }

    /// <summary>
    /// Адрес электронной почты 
    /// </summary>
    public string Email { get; set; }
}
