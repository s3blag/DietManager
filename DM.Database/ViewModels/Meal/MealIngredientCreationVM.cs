using System;
using System.ComponentModel.DataAnnotations;

namespace DM.Models.ViewModels
{
    public class MealIngredientCreationVM
    {
        public Guid? PhotoId { get; set; }

        [Required]
        [MinLength(3)]
        public string Name { get; set; }

        [Required]
        [Range(0, Int32.MaxValue)]
        public int Calories { get; set; }

        [Required]
        public NutritionsVM Nutritions { get; set; }
    }
}