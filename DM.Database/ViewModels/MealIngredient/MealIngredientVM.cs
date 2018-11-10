using System;
using System.ComponentModel.DataAnnotations;

namespace DM.Models.ViewModels
{
    public class MealIngredientVM
    {
        [Required]
        public Guid? Id { get; set; }

        public Guid? ImageId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int? Calories { get; set; }

        public NutritionsVM Nutrition { get; set; }
    }
}