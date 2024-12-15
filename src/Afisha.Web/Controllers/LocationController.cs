using Afisha.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Afisha.Web.Controllers;

[ApiController]
[Route("[controller]")]
public class LocationController : ControllerBase
{
    // private readonly IUserSomeActionService _userSomeAction;
    public LocationController( /*IUserSomeActionService userSomeAction*/)
    {
        // _userSomeAction = userSomeAction;
    }

    [HttpGet]
    public async Task<Location> Get()
    {
        // return await _userSomeAction.SomeActionAsync();
        return null;
    }
}