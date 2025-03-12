namespace Afisha.Application.Services.Interfaces.Auth

{
    public interface IPasswordHasher
    {
        string Generate(string password);
        bool Verify(string password, string passwordHash);
    }
}