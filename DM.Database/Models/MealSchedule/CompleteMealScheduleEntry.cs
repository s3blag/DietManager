using DM.Database;
using System;

namespace DM.Models
{
    public class CompleteMealScheduleEntry
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
        public Guid MealId { get; set; }
        public MealWithIngredients Meal { get; set; }
        public DateTimeOffset Date { get; set; }
    }
}
