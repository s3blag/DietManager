using System;
using System.ComponentModel.DataAnnotations;

namespace DM.Models.ViewModels
{
    public class MealScheduleEntryUpdateVM
    {
        [Required]
        public Guid? Id { get; set; }

        [Required]
        public DateTimeOffset? NewDate { get; set; }
    }
}
