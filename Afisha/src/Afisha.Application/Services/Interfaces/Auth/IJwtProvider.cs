using Afisha.Domain.Entities;

namespace Afisha.Infrastructure
{
    public interface IJwtProvider
    {
        string GenerateToken(User user);
    }
}