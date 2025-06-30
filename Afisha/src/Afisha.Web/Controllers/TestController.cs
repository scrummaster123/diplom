using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Afisha.Web.Controllers
{
    [ApiController]
    [Route("test")]
    [AllowAnonymous]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Test successful");
        }

        [HttpOptions]
        public IActionResult Options()
        {
            return Ok("OPTIONS successful");
        }
    }
}