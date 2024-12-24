using Afisha.Application.Contracts.Repositories;
using Afisha.Domain.Contracts;
using Afisha.Domain.Entities;

namespace Afisha.Application.Services
{
    // В дальнейшем лучше было бы сделать котроллер/сервис для регистрации пользователя,
    // сверстать входные и выходные модели, сделать аутентификацию/авторизацию и добавить роли
    public class UserService(IUserRepository userRepository) : IUserService
    {
        /// <summary>
        ///  Добавление нового пользователя 
        /// </summary>
        public async Task<User> AddUserAsync(User user, CancellationToken cancellationToken)
        {            
            var addedUser = await userRepository.AddUserAsync(user, cancellationToken);
            
            if (addedUser is not null)
            {
                return addedUser;
            }
            
            throw new Exception("Не удалось добавить нового пользователя");
            // + Логирование
        }

        /// <summary>
        ///  Получение информации о пользователе
        /// </summary>
        public async Task<User> GetUserByIdAsync(long id, CancellationToken cancellationToken)
        {
            var user = await userRepository.GetUserByIdAsync(id, cancellationToken);

            if (user is null)
            {
                throw new Exception($"Пользователь с идентификатором {id} не найден");
                // + Логирование
            }
            return user;
        }

        /// <summary>
        /// Удаление пользователя 
        /// </summary>
        public async Task<bool> DeleteUserAsync(long id, CancellationToken cancellationToken)
        {
            
            if ( await userRepository.DeleteUserAsync(id, cancellationToken) == true)
            {                   
                return true;
                // + Логирование
            }
            throw new Exception("Не удалось удалить пользователя");
        }
    }
}
