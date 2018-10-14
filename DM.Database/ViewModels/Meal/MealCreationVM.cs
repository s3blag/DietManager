using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DM.Models.ViewModels
{
    public class MealCreationVM
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        public Guid? PhotoId { get; set; }

        [Required]
        public int Calories { get; set; }

        [Required]
        [MinLength(2)]
        public IEnumerable<Guid> Ingredients { get; set; }
    }
}
