namespace Afisha.Application.DTO.Outputs
{
    /// <summary>
    ///     Мини модель для отображения пользователя, когда подробная информация о юзере не нужна
    /// </summary>
    public class OutputMiniUserModel
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
        ///     Полное имя пользователя
        /// </summary>
        public string FullName => $"{FirstName} {LastName}";

        /// <summary>
        ///     Логин пользователя 
        /// </summary>
        public string Login { get; set; }
    }
}
