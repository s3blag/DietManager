using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DM.Database;

namespace DM.Repositories.Interfaces
{
    public interface IFriendRepository: IBaseRepository<Friend>
    {
        Task<IEnumerable<User>> GetUserFriendsAsync(Guid userId, int index, int takeAmount, bool invitationAccepted = true);
        Task<bool> AcceptFriendInvitationAsync(Guid user1Id, Guid user2Id);
    }
}