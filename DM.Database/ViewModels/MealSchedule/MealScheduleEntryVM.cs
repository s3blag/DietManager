using System;
using System.ComponentModel.DataAnnotations;

namespace DM.Models.ViewModels
{
    public class MealScheduleEntryVM
    {
        [Required]
        public Guid? Id { get; set; }

        [Required]
        public MealVM Meal { get; set; }

        [Required]
        public DateTimeOffset? Date { get; set; }
    }
}
