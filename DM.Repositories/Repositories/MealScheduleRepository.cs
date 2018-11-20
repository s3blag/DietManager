using DM.Database;
using DM.Repositories.Extensions;
using DM.Repositories.Interfaces;
using LinqToDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DM.Repositories
{
    public class MealScheduleRepository : BaseRepository<MealScheduleEntry>, IMealScheduleRepository
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

        //public async Task<int> GetNumberOfMealUsesAsync(Guid mealId)
        //{
        //    using (var db = new DietManagerDB())
        //    {
        //        return await db.MealScheduleEntries.
        //            Where(m => m.MealId == mealId).
        //            CountAsync();
        //    }
        //}

        public async Task<int> GetMealScheduleUpdatesStreakInDaysAsync(Guid userId)
        {
            using (var db = new DietManagerDB())
            {
                var query = db.MealScheduleEntries.
                    Where(m => m.UserId == userId).
                    GroupBy(m => m.Date.SubtractWithResultInDays(DateTimeOffset.Now)).
                    OrderByDescending(_ => _.Key).
                    Select(_ => _.Key).
                    AsQueryable();

                var differenceBetweenDaysWithUpdatedScheduleAndCurrentDay = await query.ToListAsync();

                bool streakNotEnded = true;
                int dayDifferenceIndex = 0;

                while (streakNotEnded && -dayDifferenceIndex < differenceBetweenDaysWithUpdatedScheduleAndCurrentDay.Count)
                {
                    if (differenceBetweenDaysWithUpdatedScheduleAndCurrentDay[-dayDifferenceIndex] != dayDifferenceIndex)
                    {
                        streakNotEnded = false;
                    }
                    else
                    {
                        dayDifferenceIndex--;
                    }
                }

                return -dayDifferenceIndex;
            }    
        }

        public async Task<bool> DeleteAsync(Guid userId, Guid mealScheduleEntryId)
        {
            using (var db = new DietManagerDB())
            {
                db.BeginTransaction();
                bool succeeded;

                var scheduleEntryq = db.MealScheduleEntries.
                    Where(m => m.Id == mealScheduleEntryId).
                    Where(m => m.UserId == userId);

                var scheduleEntry = await scheduleEntryq.FirstOrDefaultAsync();

                if (scheduleEntry == null)
                {
                    return false;
                }

                succeeded = (await scheduleEntryq.DeleteAsync() == 1);

                if (!succeeded)
                {
                    return false;
                }

                succeeded = (await db.Meals.
                        Where(m => m.Id == scheduleEntry.MealId).
                        Set(m => m.NumberOfUses, m => m.NumberOfUses - 1).
                        UpdateAsync() == 1);
                

                db.CommitTransaction();

                return succeeded;
            }
        }

        public override async Task<bool> AddAsync(MealScheduleEntry model)
        {
            using (var db = new DietManagerDB())
            {
                db.BeginTransaction();
                bool succeeded;

                succeeded = (await db.InsertAsync(model) == 1);

                if (!succeeded)
                {
                    return false;
                }

                succeeded = (await db.Meals.
                        Where(m => m.Id == model.MealId).
                        Set(m => m.NumberOfUses, m => m.NumberOfUses + 1).
                        UpdateAsync() == 1);

                db.CommitTransaction();

                return succeeded;
            }
        }

        public async Task<bool> UpdateAsync(MealScheduleEntry model)
        {
            using (var db = new DietManagerDB())
            {
                var result = await db.MealScheduleEntries.
                    Where(m => m.Id == model.Id).
                    Where(m => m.UserId == model.UserId).
                    Set(m => m.Date, model.Date).
                    UpdateAsync();

                return result == 1;
            }
        }
    }
}
