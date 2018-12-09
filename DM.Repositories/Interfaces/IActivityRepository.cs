using DM.Database;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DM.Repositories.Interfaces
{
    public interface IActivityRepository: IBaseRepository<UserActivity>
    {
        Task<ICollection<UserActivity>> GetUserActivitiesAsync(Guid userId, int index, int takeAmount);
        Task<ICollection<UserActivity>> GetUsersFriendsActivitiesAsync(Guid userId, int index, int takeAmount);
        Task<ICollection<UserActivity>> GetAllActivitiesAsync(int index, int takeAmount);
        Task<bool> MarkAsSeenAsync(IEnumerable<int> activityIds);
    }
}