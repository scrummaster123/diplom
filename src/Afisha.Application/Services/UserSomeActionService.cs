using Afisha.Domain.Contracts;
using Afisha.Domain.Entities;

namespace Afisha.Application.Services
{
    public class UserSomeActionService : IUserSomeActionService
    {

        public async Task<User> SomeActionAsync()
        {
            await Task.Delay(1);
            return new User {Id = 1, UserName = "TEST" };
        }
    }
}
