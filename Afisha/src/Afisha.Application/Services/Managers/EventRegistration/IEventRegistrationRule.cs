namespace Afisha.Application.Services.Managers.EventRegistration;

public interface IEventRegistrationRule
{
    /// <summary>
    ///     Проверка возможности зарегистрироваться на мероприятие
    /// </summary>
    /// <param name="eventId"> Идентификатор мероприятия</param>
    /// <param name="userId"> Идентификатор пользователя</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Сообщение-описание, можно ли пользователю попасть на мероприятие</returns>
    /// TODO необходимо будет заложить расширенный функционал на правила при регистрации. Сейчас всё решается
    /// Булевой переменной: регистрация открытая или закрытая
    public Task<Message> CheckRuleAsync(long eventId, long userId, CancellationToken cancellationToken);
}