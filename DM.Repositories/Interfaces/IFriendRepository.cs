using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DM.Database;
using DM.Models.Enums;

namespace DM.Repositories.Interfaces
{
    public interface IFriendRepository: IBaseRepository<Friend>
    {
        Task<ICollection<User>> GetFriendsAsync(Guid userId, int index, int takeAmount, FriendInvitationStatus status = FriendInvitationStatus.Accepted);
        Task<ICollection<Guid>> GetFriendsIdsAsync(Guid userId, int index = 0, int takeAmount = Int32.MaxValue, FriendInvitationStatus status = FriendInvitationStatus.Accepted);
        Task<bool> SetFriendInvitationStatusAsync(Guid invitingUserId, Guid invitedUserId, FriendInvitationStatus status);
        Task<int> GetNumberOfFriendsAsync(Guid userId);
        Task<bool> RemoveFriendAsync(Guid userId, Guid friendId);
    }
}