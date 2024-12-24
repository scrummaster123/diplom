using Afisha.Application.Contracts.Repositories;
using Afisha.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Afisha.Infrastructure.Repositories
{
    public class UserRepository(AfishaDbContext context) : IUserRepository
    {
        /// <summary>
        ///  Добавление пользователя в бд
        /// </summary>
        public async Task<User?> AddUserAsync(User user, CancellationToken cancellationToken)
        {
            // Поиск логина в бд , для создания пользователя нужен уникальный логин (?)
            var result = await context.Users.FirstOrDefaultAsync(x => x.Login == user.Login);
            if (result is not null)
            {
                throw new Exception("Пользователь с таким логином уже существует");
            }

            // Добавляю пользователя в бд
            var addedUser = context.Users.Add(user);

            // При выполнении SaveChanges, возвращает пользователя
            if (await context.SaveChangesAsync(cancellationToken) > 0)
            {
                // + Логирование добавления юзера
                return addedUser.Entity;
            }

            throw new Exception("Не удалось добавить нового пользователя");
            // + Логирование ошибки
        }

        /// <summary>
        ///  Получение пользователя по id
        /// </summary>
        public async Task<User?> GetUserByIdAsync(long id, CancellationToken cancellationToken)
        {
            var user = await context.Users.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
            return user;
        }

        /// <summary>
        ///  Удаление пользователя 
        /// </summary>
        public async Task<bool> DeleteUserAsync(long id, CancellationToken cancellationToken)
        {
            try
            {
                var user = await GetUserByIdAsync(id, cancellationToken);

                if (user is null)
                {
                    throw new Exception($"Пользователь c идентификатором {id} не существует");
                }

                context.Users.Remove(user);

                await context.SaveChangesAsync(cancellationToken);
                // + Логирование удаления юзера
                return true;
            }
            catch (Exception ex)
            {
                // + Логирование ошибки
                Console.WriteLine($"Произошла ошибка при удалении пользователя {ex.Message}");
                return false;
            }
        }
    }
}
