using DM.Database;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DM.Repositories.Interfaces
{
    public interface IAchievementRepository
    {
        Task<bool> AddAchievementsAsync(IEnumerable<Achievement> achievements);
        Task<IEnumerable<Achievement>> GetAllAsync();
        Achievement GetAchievement(object achievement, int value);
    }
}
