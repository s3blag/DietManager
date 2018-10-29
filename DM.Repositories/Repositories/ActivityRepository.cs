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
        public async Task<IEnumerable<UserActivity>> GetUsersActivitiesAsync(IEnumerable<Guid> users, int index, int takeAmount, ActivityType? activityType = null)
        {
            using (var db = new DietManagerDB())
            {
                var usersActivitiesQuery = db.UserActivities.
                    Where(f => users.Contains(f.UserId)).
                    OrderBy(u => u.ActivityDate).
                    Skip(index).
                    Take(takeAmount);

                if (activityType != null)
                {
                    usersActivitiesQuery = usersActivitiesQuery.
                        Where(u => u.ActivityType == activityType.ToString());
                }

                return await usersActivitiesQuery.ToListAsync();
            }
        }
    }
}
