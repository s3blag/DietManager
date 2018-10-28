using DM.Database;
using DM.Repositories.Interfaces;
using LinqToDB;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DM.Repositories
{
    public class AchievementRepository: BaseRepository<Achievement>, IAchievementRepository
    {
        public async Task<IEnumerable<Achievement>> GetAll()
        {
            using (var db = new DietManagerDB())
            {
                return await db.Achievements.ToListAsync();
            }
        }
    }
}
