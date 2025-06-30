using Afisha.Application.Services.Interfaces;
using Afisha.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Serilog;

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

        [HttpGet]
        [Route("log-test")]
        public async Task LoggerTest()
        {
            Log.Fatal("test fatal error");
            throw new NotImplementedException();
        }
    }
}
