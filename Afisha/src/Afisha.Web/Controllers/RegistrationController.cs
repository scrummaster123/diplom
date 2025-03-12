using Afisha.Application.Services;
using Afisha.Application.DTO.Inputs;
using Afisha.Application.Services.Interfaces.Auth;
using Microsoft.AspNetCore.Mvc;

namespace Afisha.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RegistrationController(IRegistrationService IregistrationService) : Controller
    {
        [HttpPost]
        [Route("registration")]
        public static async Task<IActionResult> RegisterUserAsync(RegistrationUserModel userDto, IRegistrationService registrationService)
        {
            await registrationService.RegisterUserAsync(userDto)
        }
    }
}
