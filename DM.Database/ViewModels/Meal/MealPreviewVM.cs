using System;
using System.ComponentModel.DataAnnotations;

namespace DM.Models.ViewModels
{
    public class MealPreviewVM
    {
        [Required]
        public Guid Id { get; set; }

        public Guid? ImageId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int Calories { get; set; }

        [Required]
        public DateTimeOffset CreationDate { get; set; }
    }
}
