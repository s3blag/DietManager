using System;
using System.Threading.Tasks;
using DM.Logic.Interfaces;
using DM.Models.ViewModels;
using DM.Models.Wrappers;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class FriendsController : Controller
    {
        private readonly IFriendService _friendService;

        public FriendsController(IFriendService friendService)
        {
            _friendService = friendService;
        }

        [HttpPost("my-friends")]
        public async Task<IActionResult> GetUsersFriends([FromBody] IndexedResult<UserFriendsVM> lastReturned)
        {
            if (lastReturned != null && lastReturned.IsLast)
            {
                return NotFound("Invalid arguments");
            }

            var userId = Guid.Empty;

            var friends = await _friendService.GetUserFriendsAsync(userId, lastReturned);

            if (friends == null)
            {
                return NotFound();
            }

            return Ok(friends);
        }

        [HttpPost("invite")]
        public async Task<IActionResult> Invite([FromBody]Guid invitedUserId)
        {
            var userId = Guid.Empty;

            await _friendService.SendFriendInvitationAsync(new FriendInvitationCreationVM() { InvitedUserId = invitedUserId, InvitingUserId = userId });

            return Ok();
        }

        [HttpPost("invitations")]
        public async Task<IActionResult> GetInvitations([FromBody] IndexedResult<AwaitingFriendInvitationVM> lastReturned)
        {
            if (lastReturned != null && lastReturned.IsLast)
            {
                return NotFound("Invalid arguments");
            }

            var userId = Guid.Empty;

            var friendInvitations = await _friendService.GetFriendInvitationsAsync(userId, lastReturned);

            if (friendInvitations == null)
            {
                return NotFound();
            }

            return Ok(friendInvitations);
        }

        [HttpPost("invitations/accept")]
        public async Task<IActionResult> AcceptInvitation([FromBody]AwaitingFriendInvitationVM friendInvitationVM)
        {
            var userId = Guid.Empty;

            await _friendService.AcceptFriendInvitationAsync(friendInvitationVM, userId);

            return Ok();
        }

        [HttpPost("invitations/ignore")]
        public async Task<IActionResult> IgnoreInvitation([FromBody]AwaitingFriendInvitationVM friendInvitationVM)
        {
            var userId = Guid.Empty;

            await _friendService.IgnoreFriendInvitationAsync(friendInvitationVM, userId);

            return Ok();
        }

        [HttpPost("news-feed")]
        public async Task<IActionResult> GetNewsFeed([FromBody]IndexedResult<UserActivityVM> lastReturned)
        {
            if (lastReturned != null && lastReturned.IsLast)
            {
                return NotFound("Invalid arguments");
            }

            var userId = Guid.Empty;

            var newsFeed = await _friendService.GetFriendsActivitiesFeedAsync(userId, lastReturned);

            if (newsFeed == null)
            {
                return NotFound();
            }

            return Ok(newsFeed);
        }
    }
}
