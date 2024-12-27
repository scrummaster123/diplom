using Afisha.Application.Services.Interfaces;
using Afisha.Domain.Entities;

namespace Afisha.Application.Services;
public class UserNotificationSender : IUserNotificationSender
{
    public Task SendNotificationAsync(User to, User from, string subject, string body)
    {
        throw new NotImplementedException();
    }
}