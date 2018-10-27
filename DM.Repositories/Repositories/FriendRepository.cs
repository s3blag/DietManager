using DM.Database;
using LinqToDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DM.Repositories
{
    public class FriendRepository
    {
        public async Task<IEnumerable<User>> GetUserFriendsAsync(Guid userId, int index, int takeAmount)
        {
            using (var db = new DietManagerDB())
            {
                var userFriendsQuery = db.Friends.
                    LoadWith(f => f.User2).
                    LoadWith(f => f.User1).
                    Where(f => f.User1Id == userId || f.User2Id == userId).
                    Where(f => f.Confirmed).
                    OrderBy(f => f.CreationDate).
                    Skip(index).
                    Take(takeAmount).
                    Select(f => f.User1Id == userId ? f.User2 : f.User1);

                return await userFriendsQuery.ToListAsync();
            }
        }

        public async Task<bool> AddFriendAsync(Friend newFriend)
        {
            using (var db = new DietManagerDB())
            {
                int result = await db.InsertAsync(newFriend);

                return Convert.ToBoolean(result);
            }
        }

        public async Task<bool> DeleteFriendAsync(Friend friendToDelete)
        {
            using (var db = new DietManagerDB())
            {
                int result = await db.DeleteAsync(friendToDelete);

                return Convert.ToBoolean(result);
            }
        }
    }
}
