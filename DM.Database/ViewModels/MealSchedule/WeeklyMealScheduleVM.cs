using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DM.Models.ViewModels.MealSchedule
{
    public class WeeklyMealScheduleVM
    {
        private Dictionary<DayOfWeek, IEnumerable<MealScheduleEntryVM>> _mealSchedule;

        public void Set(DayOfWeek dayOfWeek, IEnumerable<MealScheduleEntryVM> entries)
        {
            _mealSchedule[dayOfWeek] =  entries;
        }

        public Dictionary<string, IEnumerable<MealScheduleEntryVM>> Get()
        {
            return _mealSchedule.ToDictionary(kv => kv.Key.ToString().ToLower(), kv => kv.Value);
        }
    }
}