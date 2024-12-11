using Afisha.Domain.Entities;

namespace Afisha.Application.Contracts
{
    public interface IUserNotificationSender
    {
        Task SendNotificationAsync(User to, User from, string subject, string body);
    }
}
