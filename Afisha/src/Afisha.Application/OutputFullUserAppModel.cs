using Afisha.Domain.Entities;

namespace Afisha.Application
{
    /// <summary>
    ///     Полная модель для просмотра подробной информации юзера
    /// </summary>
    public class OutputFullUserAppModel
    {
        /// <summary>
        ///     Имя пользователя
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        ///     Фамилия пользователя
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        ///     Отчество пользователя (необязательное поле)
        /// </summary>
        public string? Patronymic { get; set; }

        /// <summary>
        ///     Адрес электронной почты 
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        ///     Логин юзера
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        ///    Возраст пользователя (необязательное поле)
        /// </summary>
        public int? Age { get; set; }

        /// <summary>
        ///     Пол пользователя (необязательное поле)
        /// </summary>
        public bool? IsMale { get; set; }

        /// <summary>
        ///     Локации пользователя
        /// </summary>
        public ICollection<Location> Locations { get; set; } = Array.Empty<Location>();  // Необходима замена модели в дальнейшем

        /// <summary>
        /// События пользователя
        /// </summary>
        public ICollection<Event> Events { get; set; } = Array.Empty<Event>();  // Необходима замена модели в дальнейшем
    }
}
