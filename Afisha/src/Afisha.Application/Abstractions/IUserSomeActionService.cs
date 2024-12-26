using Afisha.Domain.Entities;

namespace Afisha.Application.Abstractions;
public interface IUserSomeActionService
{
    Task<User> SomeActionAsync();
}
