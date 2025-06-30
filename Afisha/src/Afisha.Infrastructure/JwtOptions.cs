using Afisha.Application.Services.Interfaces.Auth;

namespace Afisha.Infrastructure
{
    public class JwtOptions
    {
        public string SecretKey { get; set; } 
        public int ExpiresHours { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
    }
}
