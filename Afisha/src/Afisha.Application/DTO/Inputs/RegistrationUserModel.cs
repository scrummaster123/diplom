using System.ComponentModel.DataAnnotations;
using Afisha.Application;

namespace Afisha.Application.DTO.Inputs
{
    /// <summary>
    ///     Модель для регистрации нового пользователя в системе
    /// </summary>
    public class RegistrationUserModel
    {
        /// <summary>
        ///     Имя пользователя 
        /// </summary>        
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [StringLength(50, MinimumLength = 2)]
        public string FirstName { get; set; }

        /// <summary>
        ///     Фамилия пользователя
        /// </summary>        
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [StringLength(50, MinimumLength = 2)]
        public string LastName { get; set; }

        /// <summary>
        ///     Отчество пользователя (необязательное поле)
        /// </summary>
        [StringLength(50, MinimumLength = 2)]
        public string? Patronymic { get; set; }

        /// <summary>
        ///     Адрес электронной почты 
        /// </summary>
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [EmailAddress]
        public string Email { get; set; }

        /// <summary>
        ///     Логин для входа в аккаунт
        /// </summary>
        [Required]
        [StringLength(20, MinimumLength = 6)]
        public string Login { get; set; }

        /// <summary>
        ///     Строка для пароля
        /// </summary>
        [Required]
        [StringLength(32, MinimumLength = 8, ErrorMessage = "Пароль должен быть не менее 8 символов.")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[!$%^&*?])[A-Za-z\d!$%^&*?]{8,}$",
        ErrorMessage = "Пароль должен содержать хотя бы одну букву, одну цифру и один специальный символ.")]
        public string Password { get; set; }

        /// <summary>
        ///     Строка для подтверждения пароля
        /// </summary>
        [Required]
        [Compare("Password", ErrorMessage = "Пароли не совпадают.")]
        public string ConfirmPassword { get; set; }

        /// <summary>
        ///     Дата рождения пользователя (необязательное поле)
        /// </summary>
        [DataType(DataType.Date, ErrorMessage = "Неверный формат даты")]
        [Range(typeof(DateTime), "1925-01-01", "2025-01-01", ErrorMessage = "Дата рождения должна быть между 01.01.1925 и 01.01.2025")]
        public DateTime? Birthday { get; set; }

        /// <summary>
        ///     Пол пользователя (необязательное поле)
        /// </summary>
        public bool? IsMale { get; set; }
    }
}
