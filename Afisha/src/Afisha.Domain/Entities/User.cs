using Afisha.Domain.Entities.Abstractions;

namespace Afisha.Domain.Entities
{
    public class User : EntityBase<long>
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
        ///     Логин для входа в аккаунт
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        ///     Хешированный пароль
        /// </summary>
        public string? PasswordHash { get; set; }

        /// <summary>
        ///     Дата рождения пользователя (необязательное поле)
        /// </summary>
        public DateTime? Birthday { get; set; }

        /// <summary>
        ///     Пол пользователя (необязательное поле)
        /// </summary>
        public bool? IsMale { get; set; }
        
        /// <summary>
        ///     Локации пользователя 
        /// </summary>
        public ICollection<Location> Locations { get; set; } = [];

        /// <summary>
        ///     События пользователя 
        /// </summary>
        public ICollection<EventUser> Events { get; set; } = [];
    }
}
