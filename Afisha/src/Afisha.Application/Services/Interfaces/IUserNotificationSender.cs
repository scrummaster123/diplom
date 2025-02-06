using Afisha.Domain.Entities;

namespace Afisha.Application.Services.Interfaces
{
    public interface IUserNotificationSender
    {
        Task SendNotificationAsync(User to, User from, string subject, string body);
    }
}
