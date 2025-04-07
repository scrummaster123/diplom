using Afisha.Domain.Entities;
using Afisha.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Afisha.Domain.Interfaces.Repositories
{
    public class UserRepository(AfishaDbContext context) : IUserRepository
    {
        public async Task<User?> GetUserByLoginAsync(string login, CancellationToken cancellationToken)
        {
            return await context.Users
            .FirstOrDefaultAsync(u => u.Login == login);
        }

        public async Task<User?> GetUserByEmailAsync(string email, CancellationToken cancellationToken)
        {
            return await context.Users
            .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<bool> AddRegisterUserAsync(User user, CancellationToken cancellationToken)
        {          
            context.Users.Add(user);
            if (await context.SaveChangesAsync(cancellationToken) != 0)
            {
                return true;
            }
            return false;            
        }
    }
}
