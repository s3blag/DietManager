using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DM.Models.ViewModels
{
    public class MealVM
    {
        [Required]
        public Guid? Id { get; set; }

        public Guid? ImageId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int? Calories { get; set; }

        [Required]
        [MinLength(2)]
        public IEnumerable<MealIngredientWithQuantityVM> Ingredients { get; set; }
    }
}
