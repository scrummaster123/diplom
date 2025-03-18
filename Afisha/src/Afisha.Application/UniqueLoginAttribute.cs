using System.ComponentModel.DataAnnotations;
using Afisha.Application.Services.Interfaces;

namespace Afisha.Application
{
    public class UniqueLoginAttribute : ValidationAttribute
    {

        private readonly IUserService _userService;

        // Теперь атрибут принимает IUserService прямо через конструктор
        public UniqueLoginAttribute(IUserService userService)
        {
            _userService = userService;
        }
        // Используем синхронный метод для проверки уникальности логина
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var username = value?.ToString().ToLowerInvariant();

            if (string.IsNullOrEmpty(username))
            {
                return ValidationResult.Success; // Пропускаем, если логин пустой
            }

            // Проверяем уникальность логина
            var userExists = _userService.IsLoginAvailable(username);

            if (userExists != null)
            {
                return new ValidationResult("Этот логин уже занят.");
            }

            return ValidationResult.Success;
        }
    }
}

