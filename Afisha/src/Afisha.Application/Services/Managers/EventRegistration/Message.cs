namespace Afisha.Application.Services.Managers.EventRegistration;

public class Message
{
    public string Reason { get; init; } = string.Empty;
    public string? ApproveAdvice { get; set; }
    public RegistrationStatusEnum RequestStatus { get; set; }
    
}