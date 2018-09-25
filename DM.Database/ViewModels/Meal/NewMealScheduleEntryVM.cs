using System;

namespace DM.Models.ViewModels
{
    public class NewMealScheduleEntryVM
    {
        public Guid MealId { get; set; }

        public DateTimeOffset Date { get; set; }
    }
}
