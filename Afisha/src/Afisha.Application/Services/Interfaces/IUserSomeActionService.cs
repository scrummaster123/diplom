using Afisha.Domain.Entities;

namespace Afisha.Application.Services.Interfaces;
public interface IUserSomeActionService
{
    Task<User> SomeActionAsync();
}
