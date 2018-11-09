using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DM.Models.ViewModels;

namespace DM.Logic.Interfaces
{
    public interface IMealScheduleService
    {
        Task<Guid> AddMealScheduleEntry(Guid userId, MealScheduleEntryCreationVM newMealScheduleEntry);
        Task<Dictionary<DayOfWeek, IEnumerable<MealScheduleEntryVM>>> GetUpcomingMealSchedule(Guid userId, DateTimeOffset dateOffset);
    }
}