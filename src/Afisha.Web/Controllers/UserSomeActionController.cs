using Afisha.Domain.Contracts;
using Afisha.Domain.Entities;
using Afisha.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace Afisha.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserSomeActionController : ControllerBase
    {
        private readonly IUserSomeActionService _userSomeAction;
        public UserSomeActionController(IUserSomeActionService userSomeAction)
        {
            _userSomeAction = userSomeAction;
        }

        [HttpGet]
        public async Task<User> Get()
        {
            return await _userSomeAction.SomeActionAsync();
        }
    }
}
