using Afisha.Application.Services.Interfaces.Auth;

namespace Afisha.Infrastructure
{
    public class JwtOptions : IJwtOptions
    {
        public string SecretKey { get; set; } = string.Empty;
        public int ExpitesHours { get; set; }
    }
}
