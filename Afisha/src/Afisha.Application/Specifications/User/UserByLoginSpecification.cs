using Afisha.Domain.Entities;

namespace Afisha.Application.Specifications.User;

public class UserByLoginSpecification : SpecificationBase<Domain.Entities.User>
{
    public UserByLoginSpecification(string login) : base(
            user => user.Login == login
        )
    { }
}
