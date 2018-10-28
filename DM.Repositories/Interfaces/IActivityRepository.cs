using DM.Database;
using DM.Models.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DM.Repositories.Interfaces
{
    public interface IActivityRepository: IBaseRepository<UserActivity>
    {
        Task<IEnumerable<UserActivity>> GetUsersActivities(IEnumerable<Guid> users, int index, int takeAmount, ActivityType? activityType = null);
    }
}