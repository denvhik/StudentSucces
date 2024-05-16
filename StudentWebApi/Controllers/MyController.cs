using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace StudentWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MyController : ControllerBase
    {
        [HttpGet]
        public  async Task <IActionResult> Get()
        {
            return Ok("Hello World");
        }
        // <YOUR_GithubPersonalAccessToken_HERE
    }
}
