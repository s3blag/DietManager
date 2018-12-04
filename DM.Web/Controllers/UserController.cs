using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DM.Logic.Interfaces;
using DM.Models.ViewModels;
using DM.Models.Wrappers;
using DM.Web.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly ISearchService _searchService;
        private readonly IUserService _userService;
        private readonly ISecurityService _securityService;

        public UserController(ISearchService searchService, IUserService userService, ISecurityService securityService, IMapper mapper)
        {
            _searchService = searchService;
            _userService = userService;
            _securityService = securityService;
        }

        [HttpPost("search")]
        [Authorize]
        [ModelStateValidator]
        public async Task<IActionResult> SearchUsers([FromBody] IndexedResult<UserSearchVM> lastReturned)
        {
            if (lastReturned != null && lastReturned.IsLast)
            {
                return NotFound("Invalid arguments");
            }

            var loggedUserId = new Guid(User.Identity.Name);

            var result = await _searchService.SearchUsersAsync(loggedUserId, lastReturned);

            return Ok(result);
        }

        [HttpDelete("avatar")]
        public async Task<IActionResult> DeleteUserAvatar()
        {
            var signedInUserId = new Guid(User.Identity.Name);

            bool deleted = await _userService.DeleteAvatarAsync(signedInUserId);

            if (deleted)
            {
                return Ok();
            }

            return BadRequest();
        }

        [HttpPatch("avatar")]
        [ModelStateValidator]
        public async Task<IActionResult> UpsertUserAvatar([FromBody] AvatarVM avatar)
        {
            var signedInUserId = new Guid(User.Identity.Name);

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
            var signedInUserId = new Guid(User.Identity.Name);

            var userInfo = await _userService.GetUserInfoAsync(signedInUserId);

            if (userInfo == null)
            {
                return BadRequest();
            }

            var newAuthToken = _securityService.GenerateAuthToken(userInfo);

            var loggedInUser = new LoggedInUserVM(userInfo, newAuthToken);

            return Ok(loggedInUser);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAccount()
        {
            var signedInUserId = new Guid(User.Identity.Name);

            bool deleted = await _userService.DeleteAccountAsync(signedInUserId);

            if (deleted)
            {
                return Ok();
            }

            return BadRequest();
        }
    }
}