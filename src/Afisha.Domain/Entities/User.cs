﻿namespace Afisha.Domain.Entities
{
    public class User : EntityBase<long>
    {
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public long Id { get; set; }    //Здесь можно использовать guid или int например

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
        public string? SurName { get; set; }

        /// <summary>
        /// Адрес электронной почты 
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Логин для входа в аккаунт
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// Хешированный пароль
        /// </summary>
        public string PasswordHash { get; set; }

        /// <summary>
        /// Дата рождения пользователя (необязательное поле)
        /// </summary>
        public DateTime? Birthday { get; set; }

        /// <summary>
        /// Пол пользователя (необязательное поле)
        /// </summary>
        public bool? IsMale { get; set; }
    }
}
