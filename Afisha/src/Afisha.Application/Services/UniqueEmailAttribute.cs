using Afisha.Application.Services.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Afisha.Application.Services
{
    public class UniqueEmailAttribute
    {
        private readonly Func<IUserService> _userServiceProvider;

        // Конструктор с использованием фабрики для внедрения зависимости
        public UniqueEmailAttribute(Func<IUserService> userServiceProvider)
        {
            _userServiceProvider = userServiceProvider;
        }

        // Используем синхронный метод для проверки уникальности логина
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var email = value?.ToString().ToLowerInvariant();

            if (string.IsNullOrEmpty(email))
            {
                return ValidationResult.Success; // Пропускаем, если логин пустой
            }

            // Получаем сервис из DI через фабрику
            var userService = _userServiceProvider();

            // Проверяем уникальность логина
            var userExists = userService.GetUserByLoginAsync(email, CancellationToken.None).Result; // Синхронное ожидание результата

            if (userExists != null)
            {
                return new ValidationResult("Этот логин уже занят.");
            }

            return ValidationResult.Success;
        }
    }
}
