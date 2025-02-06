using Afisha.Application.Services.Interfaces;
using Afisha.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Afisha.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController(IUserService userService) :  Controller
    {
        [HttpPost]
        [Route("add-user")]
        public async Task<IActionResult> AddUserAsync([FromBody] User newUser)
        {
            var user = await userService.AddUserAsync(newUser, HttpContext.RequestAborted);
            return Ok(user);
        }

        [HttpGet]
        [Route("get-user-by-id")]
        public async Task<User> GetUserByIdAsync([FromQuery] long id)
        {
            return await userService.GetUserByIdAsync(id, HttpContext.RequestAborted);
        }

        [HttpDelete]
        [Route("delete-user")]
        public async Task<IActionResult> DeleteUserAsync(long id)
        {
            if( await userService.DeleteUserAsync(id, HttpContext.RequestAborted) == true)
            {
                return Ok();
            }
            return BadRequest();
        }
    }
}
