using Afisha.Application.Services.Interfaces;
using Afisha.Application.Specifications.User;
using Afisha.Domain.Entities;
using Afisha.Domain.Interfaces.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Afisha.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController(IUserService userService) : Controller
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
            if (await userService.DeleteUserAsync(id, HttpContext.RequestAborted) == true)
            {
                return Ok();
            }
            return BadRequest();
        }

        [HttpGet]
        [Route("get-user-by-login")]
        public async Task<User> GetUserByLoginAsync([FromQuery] string login)
        {
            return await userService.GetUserByLoginAsync(login, HttpContext.RequestAborted);

        }

        [HttpGet]
        [Route("get-user-by-email")]
        public async Task<User> GetUserByEmailAsync([FromQuery] string email)
        {
            return await userService.GetUserByLoginAsync(email, HttpContext.RequestAborted);
        }
    }
}
