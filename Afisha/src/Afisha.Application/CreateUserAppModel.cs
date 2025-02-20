namespace Afisha.Application
{
    public class CreateUserAppModel
    {
        /// <summary>
        ///     Имя аккаунта пользователя 
        /// </summary>
        public string AccountName { get; set; }

        /// <summary>
        ///     Имя пользователя
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        ///     Фамилия пользователя
        /// </summary>
        public string LastName { get; set; }
    }
}
