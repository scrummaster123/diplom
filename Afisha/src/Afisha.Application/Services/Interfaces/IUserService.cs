using Afisha.Domain.Entities;

namespace Afisha.Application.Services.Interfaces;
public interface IUserService
{
    /// <summary>
    ///  Добавление нового пользователя 
    /// </summary>
    Task<User> AddUserAsync(User user, CancellationToken cancellationToken);

    /// <summary>
    ///  Получение информации о пользователе
    /// </summary>
    Task<User> GetUserByIdAsync(long id, CancellationToken cancellationToken);

    /// <summary>
    /// Удаление пользователя 
    /// </summary>
    Task<bool> DeleteUserAsync(long id, CancellationToken cancellationToken);
    Task<User> GetUserByLoginAsync(string login, CancellationToken cancellationToken);
    Task<User> GetUserByEmailAsync(string email, CancellationToken cancellationToken);
}
