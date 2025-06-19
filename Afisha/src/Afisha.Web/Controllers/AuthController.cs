using Afisha.Application.DTO.Inputs;
using Afisha.Application.Services.Interfaces.Auth;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Afisha.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [AllowAnonymous]
    public class AuthController(IAuthService authService) : Controller
    {
        [HttpPost]
        [Route("user-registration")]
        public async Task<IActionResult> UserRegistrationAsync(RegistrationUserModel userModel, CancellationToken cancellationToken)
        {
            if (await authService.UserRegistrationAsync(userModel, cancellationToken))
            {
                return Ok();
            }
            return BadRequest();
        }

        [HttpPost]
        [Route("login")]
        public async Task<string> LoginAsync(LoginUserModel loginUserModel, CancellationToken cancellationToken)
        {
            var token = await authService.LoginAsync(loginUserModel.Email, loginUserModel.Password, cancellationToken);

            return token;
        }
    }
}
 