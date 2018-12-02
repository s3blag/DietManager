using System.ComponentModel.DataAnnotations;

namespace DM.Models.ViewModels
{
    public class MealIngredientWithQuantityVM
    {
        [Required]
        public double? Quantity { get; set; }

        [Required]
        public MealIngredientVM MealIngredient { get; set; }
    }
}
