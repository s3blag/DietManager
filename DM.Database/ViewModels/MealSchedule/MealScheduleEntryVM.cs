using System;
namespace DM.Models.ViewModels
{
    public class MealScheduleEntryVM
    {
        public Guid Id { get; set; }

        public MealVM Meal { get; set; }

        public DateTimeOffset Date { get; set; }
    }
}
