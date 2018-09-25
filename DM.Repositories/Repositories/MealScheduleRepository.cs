using DM.Database;
using DM.Repositories.Interfaces;
using LinqToDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DM.Repositories.Repositories
{
    public class MealScheduleRepository : IMealScheduleRepository
    {
        public async Task<IEnumerable<MealScheduleEntry>> GetMealScheduleEntriesInDateRangeAsync(Guid userId, DateTimeOffset from, DateTimeOffset to)
        {
            using (var db = new DietManagerDB())
            {
                var mealSchedule = await db.MealScheduleEntries.
                    LoadWith(m => m.Meal).
                    Where(m => m.UserId == userId).
                    Where(m => m.Date >= from && m.Date <= to).
                    ToListAsync();

                return mealSchedule.OrderBy(m => m.Date);
            }
        }

        public async Task<bool> AddMealScheduleEntryAsync(MealScheduleEntry mealScheduleEntry)
        {
            using (var db = new DietManagerDB())
            {
                var result = await db.InsertAsync(mealScheduleEntry);

                return Convert.ToBoolean(result);
            }
        }

        public async Task<bool> DeleteMealScheduleEntryAsync(Guid mealScheduleEntryId)
        {
            using (var db = new DietManagerDB())
            {
                var result = await db.MealScheduleEntries.
                    Where(m => m.Id == mealScheduleEntryId).
                    DeleteAsync();

                return Convert.ToBoolean(result);
            }
        }

    }
}
