using System;
using System.Linq;
using System.Threading.Tasks;
using DM.Logic.Interfaces;
using DM.Logic.Services;
using DM.Models.ViewModels;
using DM.Models.Wrappers;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class SocialController : Controller
    {
        public SocialController()
        {

        }

        [HttpPost("get-friends")]
        public async Task<IActionResult> GetUserFriends([FromBody] IndexedResult<UserFriendsVM> lastReturned)
        {
            if (lastReturned != null && lastReturned.IsLast)
            {
                return NotFound("Invalid arguments");
            }

            //var result = await _userService.SearchUsersAsync(lastReturned);

            //if (!result.Result.Any())
            //{
            //    return NotFound();
            //}

            return Ok();
        }
    }
}
