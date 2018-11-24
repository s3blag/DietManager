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
    public class ActivityRepository : BaseRepository<UserActivity>, IActivityRepository
    {
        public async Task<ICollection<UserActivity>> GetUserActivitiesAsync(Guid userId, int index, int takeAmount)
        {
            using (var db = new DietManagerDB())
            {
                var activitiesQuery = db.UserActivities.
                    LoadWith(ua => ua.User).
                    LoadWith(ua => ua.Meal).
                    LoadWith(ua => ua.MealIngredient).
                    LoadWith(ua => ua.Favourite).
                    LoadWith(ua => ua.Friend).
                    LoadWith(ua => ua.Achievement).
                    Where(ua => userId == ua.UserId).
                    OrderBy(ua => ua.Id).
                    Skip(index).
                    Take(takeAmount);

                return await activitiesQuery.ToListAsync();
            }
        }

        public async Task<ICollection<UserActivity>> GetUsersFriendsActivitiesAsync(Guid userId, int index, int takeAmount)
        {
            using (var db = new DietManagerDB())
            {
                var activitiesQuery = db.UserActivities.
                    LoadWith(ua => ua.User).
                    LoadWith(ua => ua.Meal).
                    LoadWith(ua => ua.MealIngredient).
                    LoadWith(ua => ua.Favourite).
                    LoadWith(ua => ua.Friend).
                    LoadWith(ua => ua.Achievement).
                    Where(ua => GetFriendsIdsQuery(db, userId).Contains(ua.UserId)).
                    OrderBy(ua => ua.Id).
                    Skip(index).
                    Take(takeAmount);

                return await activitiesQuery.ToListAsync();
            }
        }

        private IQueryable<Guid> GetFriendsIdsQuery(DietManagerDB db, Guid userId)
        {
            return db.Friends.
                Where(f => f.InvitedUserId == userId || f.InvitingUserId == userId).
                Where(f => f.Status == FriendInvitationStatus.Accepted.ToString()).
                Select(f => f.InvitingUserId == userId ? f.InvitedUserId : f.InvitingUserId);
        }

    }
}
