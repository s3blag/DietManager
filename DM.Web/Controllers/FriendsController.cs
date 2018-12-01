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
        public async Task<IActionResult> GetUsersFriends([FromBody] IndexedResult<UserVM> lastReturned)
        {
            if (lastReturned != null && lastReturned.IsLast)
            {
                return NotFound("Invalid arguments");
            }

            var userId = Guid.Empty;

            var friends = await _friendService.GetFriendsAsync(userId, lastReturned);

            if (friends == null)
            {
                return NotFound();
            }

            return Ok(friends);
        }

        [HttpPost("invite")]
        public async Task<IActionResult> Invite([FromBody] FriendInvitationCreationVM invitation)
        {
            var userId = Guid.Empty;

            invitation.InvitingUserId = userId;

            await _friendService.SendFriendInvitationAsync(invitation);

            return Ok();
        }

        [HttpGet("{friendId}")]
        public async Task<IActionResult> GetFriend(Guid friendId)
        {
            var userId = Guid.Empty;

            var result = await _friendService.GetFriendWithAchievementsAsync(userId, friendId);

            if (result == null || result.User == null)
            {
                return NotFound();
            }

            return Ok(result);
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
        public async Task<IActionResult> AcceptInvitation([FromBody]InvitationAction invitationData)
        {
            var userId = Guid.Empty;

            await _friendService.AcceptFriendInvitationAsync(invitationData.InvitingUserId.Value, userId);

            return Ok();
        }

        [HttpPost("invitations/ignore")]
        public async Task<IActionResult> IgnoreInvitation([FromBody]InvitationAction invitationData)
        {
            var userId = Guid.Empty;

            await _friendService.IgnoreFriendInvitationAsync(invitationData.InvitingUserId.Value, userId);

            return Ok();
        }

        [HttpDelete("remove/{friendId}")]
        public async Task<IActionResult> Remove(Guid friendId)
        {
            var userId = Guid.Empty;

            await _friendService.RemoveFromFriendsAsync(friendId, userId);

            return Ok();
        }

        [HttpPost("news-feed")]
        public async Task<IActionResult> GetNewsFeed([FromBody]IndexedResult<UserActivityVM> lastReturned)
        {
            if (lastReturned != null && lastReturned.IsLast)
            {
                return NotFound("Invalid arguments");
            }

            var userId = new Guid("00000000-0000-0000-0000-000000000000");

            var newsFeed = await _friendService.GetFriendsActivitiesFeedAsync(userId, lastReturned);

            return Ok(newsFeed);
        }
    }
}
