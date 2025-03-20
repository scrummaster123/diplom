namespace Afisha.Infrastructure
{
    public class PasswordHasher : IPasswordHasher
    {
        /// <summary>
        /// Метод хеширует пароль с помощью кодировки SHA384 и генерирует соль с помощью метода библиотеки
        /// </summary>
        public string Generate(string password) =>
            BCrypt.Net.BCrypt.EnhancedHashPassword(password);

        /// <summary>
        /// Метод сопоставляет пароль и хеш-пароль и возвращает результат проверки
        /// </summary>
        public bool Verify(string password, string hashedPassword) =>
            BCrypt.Net.BCrypt.EnhancedVerify(password, hashedPassword);
    }
}
