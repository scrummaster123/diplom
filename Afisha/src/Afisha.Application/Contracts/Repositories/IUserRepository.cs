using Afisha.Domain.Entities;

namespace Afisha.Application.Contracts.Repositories
{
    public interface IUserRepository
    {
        /// <summary>
        ///  Добавление пользователя в бд
        /// </summary>
        Task<User?> AddUserAsync(User user, CancellationToken cancellationToken);

        /// <summary>
        ///  Получение пользователя по id
        /// </summary>
        Task<User?> GetUserByIdAsync(long id, CancellationToken cancellationToken);

        /// <summary>
        ///  Удаление пользователя 
        /// </summary>
        Task<bool> DeleteUserAsync(long id, CancellationToken cancellationToken);
    }
}
