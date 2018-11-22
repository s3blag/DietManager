using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DM.Models.ViewModels;
using DM.Models.Wrappers;

namespace DM.Logic.Interfaces
{
    public interface IFriendService
    {
        Task<IndexedResult<IEnumerable<UserActivityVM>>> GetFriendsActivitiesFeedAsync(Guid userId, IndexedResult<UserActivityVM> lastReturned, int takeAmount = Constants.DEFAULT_DB_TAKE_VALUE);
        Task<IndexedResult<IEnumerable<UserVM>>> GetUserFriendsAsync(Guid userId, IndexedResult<UserVM> lastReturned, int takeAmount = Constants.DEFAULT_DB_TAKE_VALUE);
        Task<IndexedResult<IEnumerable<AwaitingFriendInvitationVM>>> GetFriendInvitationsAsync(Guid userId, IndexedResult<AwaitingFriendInvitationVM> lastReturned, int takeAmount = Constants.DEFAULT_DB_TAKE_VALUE);
        Task SendFriendInvitationAsync(FriendInvitationCreationVM friendInvitation);
        Task IgnoreFriendInvitationAsync(Guid invitingUserId, Guid invitedUserId);
        Task AcceptFriendInvitationAsync(Guid invitingUserId, Guid invitedUserId);
        Task RemoveFromFriends(Guid friendId, Guid userId);
    }
}