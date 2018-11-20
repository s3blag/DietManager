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
        Task<bool> SetFriendInvitationStatusAsync(Guid user1Id, Guid user2Id, FriendInvitationStatus status);
        Task<int> GetNumberOfFriendsAsync(Guid userId);
    }
}