namespace Afisha.Application.Services.Managers.EventRegistration;

public enum RegistrationStatusEnum
{
    NotSet = -1,
    OpenRegistration = 0,
    CantRegister = 1,
    AlreadyExists = 2,
    ClosedRegistration = 3
}