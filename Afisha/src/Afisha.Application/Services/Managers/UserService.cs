﻿using Afisha.Application.DTO.Outputs;
using Afisha.Application.Services.Interfaces;
using Afisha.Application.Specifications.User;
using Afisha.Domain.Entities;
using Afisha.Domain.Interfaces;
using Afisha.Domain.Interfaces.Repositories;
using AutoMapper;

namespace Afisha.Application.Services.Managers
{
    // В дальнейшем лучше было бы сделать котроллер/сервис для регистрации пользователя,
    // сверстать входные и выходные модели, сделать аутентификацию/авторизацию и добавить роли
    public class UserService(IRepository<User, long> userRepository, IUnitOfWork unitOfWork, IUserRepository userRep, IMapper mapper) : IUserService
    {
        /// <summary>
        ///  Добавление нового пользователя 
        /// </summary>
        public async Task<User> AddUserAsync(User user, CancellationToken cancellationToken)
        {
            var existingUser = await userRepository.GetAsync(new UserByLoginSpecification(user.Login), cancellationToken: cancellationToken);
            if (existingUser is not null)
                throw new Exception("Пользователь с таким логином уже существует");

            var addedUser = userRepository.Add(user);
            var affectedRows = await unitOfWork.CommitAsync(cancellationToken);

            if (affectedRows > 0)
                return addedUser;

            throw new Exception("Не удалось добавить нового пользователя");
            // + Логирование
        }

        /// <summary>
        ///  Получение информации о пользователе
        /// </summary>
        public async Task<User> GetUserByIdAsync(long id, CancellationToken cancellationToken)
        {
            var user = await userRepository.GetByIdOrThrowAsync(id, cancellationToken: cancellationToken);

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
            await userRepository.DeleteAsync(id, cancellationToken: cancellationToken);
            var affectedRows = await unitOfWork.CommitAsync(cancellationToken);

            if (affectedRows > 0)
            {
                return true;
                // + Логирование
            }
            throw new Exception("Не удалось удалить пользователя");
        }

        /// <summary>
        /// Метод получения пользователя по логину 
        /// </summary>
        public async Task<User> GetUserByLoginAsync(string login, CancellationToken cancellationToken)
        {
            var user = await userRep.GetUserByLoginAsync(login, cancellationToken);

            if (user is null)
            {
                throw new Exception($"Пользователь с логином {login} не найден");
                // + Логирование
            }           
            return user;
        }

        public async Task<User> GetUserByEmailAsync(string email, CancellationToken cancellationToken)
        {
            var user = await userRep.GetUserByLoginAsync(email, cancellationToken);

            if (user is null)
            {
                throw new Exception($"Пользователь с email {email} не найден");
                // + Логирование
            }            
            return user;
        }
    }
}
