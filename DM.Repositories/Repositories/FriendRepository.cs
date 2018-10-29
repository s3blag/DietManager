using DM.Database;
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
        public async Task<IEnumerable<User>> GetUserFriendsAsync(Guid userId, int index, int takeAmount, bool invitationAccepted = true)
        {
            using (var db = new DietManagerDB())
            {
                var userFriendsQuery = db.Friends.
                    LoadWith(f => f.User2).
                    LoadWith(f => f.User1).
                    Where(f => f.User1Id == userId || f.User2Id == userId).
                    Where(f => f.Confirmed == invitationAccepted).
                    OrderBy(f => f.CreationDate).
                    Skip(index).
                    Take(takeAmount).
                    Select(f => f.User1Id == userId ? f.User2 : f.User1);

                return await userFriendsQuery.ToListAsync();
            }
        }

        public async Task<bool> AcceptFriendInvitationAsync(Guid user1Id, Guid user2Id)
        {
            using (var db = new DietManagerDB())
            {
                var updateResult = await db.Friends.
                    Where(f => (f.User1Id == user1Id && f.User2Id == user2Id) || (f.User1Id == user2Id && f.User2Id == user1Id)).
                    Where(f => !f.Confirmed).
                    Set(f => f.Confirmed, true).
                    UpdateAsync();

                return Convert.ToBoolean(updateResult);
            }
        }

        public async Task<int> GetNumberOfFriendsAsync(Guid userId)
        {
            using (var db = new DietManagerDB())
            {
                return await db.Friends.
                    Where(f => f.User1Id == userId || f.User2Id == userId).
                    Where(f => f.Confirmed).
                    CountAsync();
            }
        }
    }
}
