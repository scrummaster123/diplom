using Afisha.Domain.Entities;

namespace Afisha.Application.Abstractions
{
    public interface IUserNotificationSender
    {
        Task SendNotificationAsync(User to, User from, string subject, string body);
    }
}
