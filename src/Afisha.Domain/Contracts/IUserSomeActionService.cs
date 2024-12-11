using Afisha.Domain.Entities;

namespace Afisha.Domain.Contracts
{
    public interface IUserSomeActionService
    {
        Task<User> SomeActionAsync();
    }
}
