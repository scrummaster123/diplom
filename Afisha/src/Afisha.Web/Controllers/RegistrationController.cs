using Afisha.Application.DTO.Inputs;
using Afisha.Application.Services.Interfaces.Auth;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Afisha.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RegistrationController(IRegistrationService registrationService) : Controller
    {
        [HttpPost]
        [Route("user-registration")]
        public async Task<IActionResult> UserRegistrationAsync(RegistrationUserModel userModel)
        {
            await registrationService.UserRegistrationAsync(userModel);
        }
    }
}
 