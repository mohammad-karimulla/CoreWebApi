using Microsoft.AspNetCore.Mvc;
namespace ServerlessAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Serverless API started...");
        }
    }
}
