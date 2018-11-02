using DM.Database;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DM.Repositories.Interfaces
{
    public interface IUserAchievementRepository: IBaseRepository<UserAchievement>
    {
        Task<bool> MarkAsReadAsync(IEnumerable<Guid> achievementIds, Guid userId);
        Task<IEnumerable<Achievement>> GetUsersAchievementsAsync(Guid userId);
        Task<int> GetUserAchievementMaxValueAsync<T>(Guid userId, T achievement);
    }
}