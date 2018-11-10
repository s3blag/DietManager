using System;
using System.ComponentModel.DataAnnotations;

namespace DM.Models.ViewModels
{
    public class MealScheduleEntryCreationVM
    {
        [Required]
        public Guid? MealId { get; set; }

        [Required]
        public DateTimeOffset? Date { get; set; }
    }
}
