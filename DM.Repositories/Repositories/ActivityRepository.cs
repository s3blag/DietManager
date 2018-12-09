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
                var activitiesQuery = GetActivitiesQuery(db).
                    Where(ua => userId == ua.UserId).
                    Where(ua => !ua.Meal.Deleted).
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
                var activitiesQuery = GetActivitiesQuery(db).
                    Where(ua => GetFriendsIdsQuery(db, userId).Contains(ua.UserId)).
                    OrderBy(ua => ua.Id).
                    Skip(index).
                    Take(takeAmount);

                return await activitiesQuery.ToListAsync();
            }
        }

        public async Task<ICollection<UserActivity>> GetAllActivitiesAsync(int index, int takeAmount)
        {
            using (var db = new DietManagerDB())
            {
                var activitiesQuery = GetActivitiesQuery(db).
                    Where(ua => !ua.SeenByAdmin).
                    OrderBy(ua => ua.Id).
                    Skip(index).
                    Take(takeAmount);

                return await activitiesQuery.ToListAsync();
            }
        }

        public async Task<bool> MarkAsSeenAsync(IEnumerable<int> activityIds)
        {
            using (var db = new DietManagerDB())
            {
                var activitiesQuery = GetActivitiesQuery(db).
                    Where(ua => activityIds.Contains(ua.Id)).
                    Set(ua => ua.SeenByAdmin, true);                    

                return (await activitiesQuery.UpdateAsync()) == activityIds.Count();
            }
        }

        private IQueryable<UserActivity> GetActivitiesQuery(DietManagerDB db)
        {
            return db.UserActivities.
                    LoadWith(ua => ua.User).
                    LoadWith(ua => ua.Meal).
                    LoadWith(ua => ua.MealIngredient).
                    LoadWith(ua => ua.Favourite).
                    LoadWith(ua => ua.Friend).
                    LoadWith(ua => ua.Achievement).
                    Where(ua => !(ua.Meal != null ? ua.Meal.Deleted : false)).
                    Where(ua => !(ua.Friend != null ? ua.Friend.Deleted : false)).
                    Where(ua => !(ua.MealIngredient != null ? ua.MealIngredient.Deleted : false)).
                    Where(ua => !(ua.Favourite != null ? ua.Favourite.Deleted : false)).
                    Where(ua => !(ua.User != null ? ua.User.Deleted : false));
        }

        private IQueryable<Guid> GetFriendsIdsQuery(DietManagerDB db, Guid userId)
        {
            return db.Friends.
                LoadWith(f => f.InvitedUser).
                LoadWith(f => f.InvitingUser).
                Where(f => !f.InvitingUser.Deleted && !f.InvitedUser.Deleted).
                Where(f => f.InvitedUserId == userId || f.InvitingUserId == userId).
                Where(f => f.Status == FriendInvitationStatus.Accepted.ToString()).
                Select(f => f.InvitingUserId == userId ? f.InvitedUserId : f.InvitingUserId);
        }

    }
}
