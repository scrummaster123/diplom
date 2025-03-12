using Afisha.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Afisha.Domain.Interfaces.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DbContext _context;

        private readonly IUserRepository _userRepository;

        public UserRepository(DbContext context, IUserRepository userRepository)
        {
            _context = context;
            _userRepository = userRepository;
        }

        public async Task<User> GetUserByLoginAsync(string login, CancellationToken cancellationToken) /////////////////????????
        {
            return await _context.Users
            .FirstOrDefaultAsync(u => u.Login == login);
        }

        public async Task<User> GetUserByEmailAsync(string email, CancellationToken cancellationToken)////////////////////?????
        {
            return await _context.Users
            .FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}
