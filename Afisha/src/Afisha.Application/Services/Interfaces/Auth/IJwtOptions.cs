namespace Afisha.Application.Services.Interfaces.Auth;
public interface IJwtOptions
{
    int ExpitesHours { get; set; }
    string SecretKey { get; set; }
}