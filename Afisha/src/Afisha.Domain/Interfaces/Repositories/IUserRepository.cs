using Afisha.Domain.Entities;

namespace Afisha.Domain.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetUserByLoginAsync(string login, CancellationToken cancellationToken);
        Task<User> GetUserByEmailAsync(string email, CancellationToken cancellationToken);
        bool IsLoginAvailable(string login);
        bool IsEmailAvailable(string email);
    }
}