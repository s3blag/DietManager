using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DM.Database;
using DM.Models.Enums;

namespace DM.Repositories.Interfaces
{
    public interface IFriendRepository: IBaseRepository<Friend>
    {
        Task<ICollection<User>> GetUserFriendsAsync(Guid userId, int index, int takeAmount, FriendInvitationStatus status = FriendInvitationStatus.Accepted);
        Task<bool> SetFriendInvitationStatusAsync(Guid invitingUserId, Guid invitedUserId, FriendInvitationStatus status);
        Task<int> GetNumberOfFriendsAsync(Guid userId);
        Task<bool> RemoveFriendAsync(Guid userId, Guid friendId);
    }
}