namespace Afisha.Application
{
    public class OutputUserAppModel
    {
        /// <summary>
        ///     Идентификатор пользователя
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        ///     Имя пользователя 
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        ///     Фамилия пользователя
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        ///     Полное имя пользователя (может быть сочетанием имени и фамилии)
        /// </summary>
        public string FullName => $"{FirstName} {LastName}";

        /// <summary>
        ///     Имя аккаунта пользователя
        /// </summary>
        public string AccountName { get; set; }
    }
}
