namespace Afisha.Application.DTO.Outputs
{
    /// <summary>
    ///     Мини модель для отображения пользователя, когда подробная информация о юзере не нужна
    /// </summary>
    public class OutputMiniUserModel
    {        
        /// <summary>
        ///     Полное имя пользователя
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        ///     Логин пользователя 
        /// </summary>
        public string Login { get; set; }
    }
}
