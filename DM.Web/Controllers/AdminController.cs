using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class AdminController : Controller
    {
        [HttpGet("get")]
        public IActionResult Get()
        {
            return Ok(new[] { 1, 2, 3 });
        }
        
    }
}