using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IMS.Api.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthTestController : ControllerBase
    {
        [HttpGet("public")]
        public IActionResult Public() => Ok("public ok");

        [Authorize]
        [HttpGet("protected")]
        public IActionResult Protected() => Ok("protected ok");
    }
}
