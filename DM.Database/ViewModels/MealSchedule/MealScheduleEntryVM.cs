using System;
using System.ComponentModel.DataAnnotations;

namespace DM.Models.ViewModels
{
    public class MealScheduleEntryVM
    {
        [Required]
        public Guid? Id { get; set; }

        public MealVM Meal { get; set; }

        public DateTimeOffset? Date { get; set; }
    }
}
