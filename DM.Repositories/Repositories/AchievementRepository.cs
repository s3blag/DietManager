using DM.Database;
using DM.Repositories.Interfaces;
using LinqToDB;
using LinqToDB.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DM.Repositories
{
    public class AchievementRepository: IAchievementRepository
    {
        private readonly ICacheContainer _achievmentsCacheContainer;

        public AchievementRepository(ICacheContainer achievementsContainer)
        {
            _achievmentsCacheContainer = achievementsContainer;
        }

        public async Task<bool> AddAchievementsAsync(IEnumerable<Achievement> achievements)
        {
            _achievmentsCacheContainer.Fill(achievements);

            return await Task.Run(() =>
            {
                using (var db = new DietManagerDB())
                {
                    var rowsCopied = db.BulkCopy(achievements);

                    return achievements.Count() == (int)rowsCopied.RowsCopied;
                }
            });
        }

        public async Task<IEnumerable<Achievement>> GetAllAsync()
        {
            using (var db = new DietManagerDB())
            {
                var achievements = await db.Achievements.ToListAsync();

                if (achievements.Any())
                {
                    _achievmentsCacheContainer.Fill(achievements);
                }

                return achievements;
            }
        }

        public Achievement GetAchievement(object achievement, int value)
        {
            return _achievmentsCacheContainer.Get(achievement, value);
        }
    }
}
