using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DM.Database;
using DM.Models;

namespace DM.Repositories.Interfaces
{
    public interface IMealScheduleRepository : IBaseRepository<MealScheduleEntry>
    {
        Task<IEnumerable<CompleteMealScheduleEntry>> GetMealScheduleEntriesInDateRangeAsync(Guid userId, DateTimeOffset from, DateTimeOffset to);
        Task<MealScheduleEntry> GetByIdAsync(Guid userId, Guid mealScheduleEntryId, bool deepLoading = false);
        Task<int> GetMealScheduleUpdatesStreakInDaysAsync(Guid userId);
        Task<bool> DeleteAsync(Guid userId, Guid mealScheduleEntryId);
        Task<bool> UpdateAsync(MealScheduleEntry model);
    }
}