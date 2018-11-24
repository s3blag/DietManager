using DM.Database;
using DM.Models.Enums;
using DM.Repositories.Interfaces;
using LinqToDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DM.Repositories
{
    public class FriendRepository : BaseRepository<Friend>, IFriendRepository
    {
        public async Task<ICollection<User>> GetFriendsAsync(Guid userId, int index, int takeAmount, FriendInvitationStatus status = FriendInvitationStatus.Accepted)
        {
            using (var db = new DietManagerDB())
            {
                var userFriendsQuery = db.Friends.
                    LoadWith(f => f.InvitingUser).
                    LoadWith(f => f.InvitedUser).
                    Where(f => f.InvitingUserId == userId || f.InvitedUserId == userId).
                    Where(f => f.Status == status.ToString()).
                    OrderBy(f => f.CreationDate).
                    Skip(index).
                    Take(takeAmount).
                    Select(f => f.InvitingUserId == userId ? f.InvitedUser : f.InvitingUser);

                return await userFriendsQuery.ToListAsync();
            }
        }

        public async Task<ICollection<Guid>> GetFriendsIdsAsync(Guid userId, int index = 0, int takeAmount = Int32.MaxValue, FriendInvitationStatus status = FriendInvitationStatus.Accepted)
        {
            using (var db = new DietManagerDB())
            {
                var userFriendsQuery = db.Friends.
                    Where(f => f.InvitingUserId == userId || f.InvitedUserId == userId).
                    Where(f => f.Status == status.ToString()).
                    OrderBy(f => f.CreationDate).
                    Skip(index).
                    Take(takeAmount).
                    Select(f => f.InvitingUserId == userId ? f.InvitedUserId : f.InvitingUserId);

                return await userFriendsQuery.ToListAsync();
            }
        }

        public async Task<bool> SetFriendInvitationStatusAsync(Guid invitingUserId, Guid invitedUserId, FriendInvitationStatus status)
        {
            using (var db = new DietManagerDB())
            {
                var updateResult = await db.Friends.
                    Where(f => (f.InvitingUserId == invitingUserId && f.InvitedUserId == invitedUserId)).
                    Where(f => f.Status != status.ToString()).
                    Set(f => f.Status, status.ToString()).
                    UpdateAsync();

                return Convert.ToBoolean(updateResult);
            }
        }

        public async Task<int> GetNumberOfFriendsAsync(Guid userId)
        {
            using (var db = new DietManagerDB())
            {
                return await db.Friends.
                    Where(f => f.InvitingUserId == userId || f.InvitedUserId == userId).
                    Where(f => f.Status == FriendInvitationStatus.Accepted.ToString()).
                    CountAsync();
            }
        }

        public async Task<bool> RemoveFriendAsync(Guid userId, Guid friendId)
        {
            using (var db = new DietManagerDB())
            {
                var rowsAffected = await db.Friends.
                    Where(f => (f.InvitingUserId == userId && f.InvitedUserId == friendId) || (f.InvitedUserId == userId && f.InvitingUserId == friendId)).
                    DeleteAsync();

                return rowsAffected == 1;
            }
        }
    }
}
