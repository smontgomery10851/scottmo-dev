using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ScottMo.Web.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class HelloController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get() => Ok(new { message = "Hello from ScottMo.Web API" });

        [Authorize]
        [HttpGet("secure")]
        public IActionResult Secure() => Ok(new { message = "This is a secure endpoint." });
    }
}
