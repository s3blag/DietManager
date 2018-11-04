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
        Task<IndexedResult<IEnumerable<UserFriendsVM>>> GetUserFriendsAsync(Guid userId, IndexedResult<UserFriendsVM> lastReturned, int takeAmount = Constants.DEFAULT_DB_TAKE_VALUE);
        Task<IndexedResult<IEnumerable<AwaitingFriendInvitationVM>>> GetFriendInvitationsAsync(Guid userId, IndexedResult<AwaitingFriendInvitationVM> lastReturned, int takeAmount = Constants.DEFAULT_DB_TAKE_VALUE);
        Task SendFriendInvitationAsync(FriendInvitationCreationVM friendInvitation);
        Task IgnoreFriendInvitationAsync(AwaitingFriendInvitationVM friendInvitation, Guid receiverId);
        Task AcceptFriendInvitationAsync(AwaitingFriendInvitationVM friendInvitation, Guid receiverId);
    }
}