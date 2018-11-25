using DM.Database;
using DM.Repositories.Interfaces;
using LinqToDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DM.Repositories
{
    public class UserAchievementRepository: BaseRepository<UserAchievement>, IUserAchievementRepository
    {
        public async Task<bool> MarkAsReadAsync(IEnumerable<Guid> achievementIds, Guid userId)
        {
            using (var db = new DietManagerDB())
            {
                var rowsAffectedCount = await db.UserAchievements.
                    Where(ua => achievementIds.Contains(ua.Id)).
                    Where(ua => ua.UserId == userId).
                    Set(ua => ua.Seen, true).
                    UpdateAsync();

                return achievementIds.Count() == rowsAffectedCount;
            }
        }

        public async Task<IEnumerable<Achievement>> GetUsersAchievementsAsync(Guid userId)
        {
            using (var db = new DietManagerDB())
            {
                return await db.UserAchievements.
                    LoadWith(ua => ua.Achievement).
                    Where(ua => ua.UserId == userId).
                    OrderBy(ua => ua.Achievement.Category).
                    ThenBy(ua => ua.Achievement.Type).
                    ThenBy(ua => ua.Achievement.Value).
                    Select(ua => ua.Achievement).
                    ToListAsync();
            }
        }

        public async Task<int> GetUserAchievementMaxValueAsync<T>(Guid userId, T achievement)
        {
            using (var db = new DietManagerDB())
            {
                var dbQuery = db.UserAchievements.
                    LoadWith(ua => ua.Achievement).
                    Where(ua => ua.UserId == userId).
                    Where(ua => ua.Achievement.Category == achievement.GetType().Name).
                    Where(ua => ua.Achievement.Type == achievement.ToString()).
                    Select(ua => (int?)ua.Achievement.Value);

                var maxValue = await dbQuery.MaxAsync();

                return maxValue.GetValueOrDefault();
            }
        }
    }
}
