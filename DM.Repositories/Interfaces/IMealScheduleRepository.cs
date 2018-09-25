using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DM.Database;

namespace DM.Repositories.Interfaces
{
    public interface IMealScheduleRepository
    {
        Task<bool> AddMealScheduleEntryAsync(MealScheduleEntry mealScheduleEntry);
        Task<bool> DeleteMealScheduleEntryAsync(Guid mealScheduleEntryId);
        Task<IEnumerable<MealScheduleEntry>> GetMealScheduleEntriesInDateRangeAsync(Guid userId, DateTimeOffset from, DateTimeOffset to);
    }
}