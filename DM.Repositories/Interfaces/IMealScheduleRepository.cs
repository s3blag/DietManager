using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DM.Database;

namespace DM.Repositories.Interfaces
{
    public interface IMealScheduleRepository : IBaseRepository<MealScheduleEntry>
    {
        Task<IEnumerable<MealScheduleEntry>> GetMealScheduleEntriesInDateRangeAsync(Guid userId, DateTimeOffset from, DateTimeOffset to);
        Task<int> GetNumberOfMealUsesAsync(Guid mealId);
        Task<int> GetMealScheduleUpdatesStreakInDaysAsync(Guid userId);
        Task<bool> DeleteAsync(Guid userId, Guid mealScheduleEntryId);
        Task<bool> UpdateAsync(MealScheduleEntry model);
    }
}