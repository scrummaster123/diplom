using Afisha.Domain.Entities;

namespace Afisha.Application.DTO.Outputs
{
    /// <summary>
    ///     Полная модель для просмотра подробной информации юзера
    /// </summary>
    public class OutputFullUserModel
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
        ///     Логин юзера
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        ///    Возраст пользователя (необязательное поле)
        /// </summary>
        public int? Age { get; set; }

        /// <summary>
        ///     Пол пользователя (необязательное поле)
        /// </summary>
        public bool? IsMale { get; set; }

        /// <summary>
        ///     Локации пользователя
        /// </summary>
        public ICollection<Location> Locations { get; set; } = [];  // ------------ (?) Замена модели на  ? ------------

        /// <summary>
        /// События пользователя
        /// </summary>
        public ICollection<Event> Events { get; set; } = [];  // ----- (?) Здесь нужна замена на OutputEvent --------
    }
}
