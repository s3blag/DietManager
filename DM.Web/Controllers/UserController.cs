using System;
using System.Linq;
using System.Threading.Tasks;
using DM.Logic.Interfaces;
using DM.Models.ViewModels;
using DM.Models.Wrappers;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly ISearchService _searchService;
        private readonly IUserService _userService;

        public UserController(ISearchService searchService, IUserService userService)
        {
            _searchService = searchService;
            _userService = userService;
        }

        [HttpPost("search")]
        public async Task<IActionResult> SearchUsers([FromBody] IndexedResult<UserSearchVM> lastReturned)
        {
            if (lastReturned != null && lastReturned.IsLast)
            {
                return NotFound("Invalid arguments");
            }

            var loggedUserId = Guid.Empty;

            var result = await _searchService.SearchUsersAsync(loggedUserId, lastReturned);

            return Ok(result);
        }

        [HttpDelete("avatar")]
        public async Task<IActionResult> DeleteUserAvatar()
        {
            var signedInUserId = Guid.Empty;

            bool deleted = await _userService.DeleteAvatarAsync(signedInUserId);

            if (deleted)
            {
                return Ok();
            }

            return BadRequest();
        }

        [HttpPatch("avatar")]
        public async Task<IActionResult> UpsertUserAvatar([FromBody] AvatarVM avatar)
        {
            //if (avatar.ImageId == Guid.Empty)
            //{
            //    return BadRequest();
            //}

            var signedInUserId = Guid.Empty;

            bool upserted = await _userService.UpsertAvatarAsync(signedInUserId, avatar.ImageId);

            if (upserted)
            {
                return Ok(avatar.ImageId);
            }

            return BadRequest();
        }

        [HttpGet]
        public async Task<IActionResult> GetUserInfo()
        {
            var signedInUserId = Guid.Empty;

            var userInfo = await _userService.GetUserInfoAsync(signedInUserId);

            if (userInfo != null)
            {
                return Ok(userInfo);
            }

            return NotFound();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAccount()
        {
            var signedInUserId = Guid.Empty;

            bool deleted = await _userService.DeleteAccountAsync(signedInUserId);

            if (deleted)
            {
                return Ok();
            }

            return BadRequest();
        }
    }
}