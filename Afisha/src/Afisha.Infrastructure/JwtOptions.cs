using Afisha.Application.Services.Interfaces.Auth;

namespace Afisha.Infrastructure
{
    public class JwtOptions : IJwtOptions
    {
        public string SecretKey { get; set; } = "Xj9sT7uYzZ8tFq3kKfGnLpV2QaRwE7x/oBmP0ZvJcNqH2MA=";
        public int ExpitesHours { get; set; } = 80;
    }
}
