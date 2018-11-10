using System.ComponentModel.DataAnnotations;

namespace DM.Models.ViewModels
{
    public class MealIngredientWithQuantityVM
    {
        [Required]
        public int? Quantity { get; set; }

        [Required]
        public MealIngredientVM MealIngredient { get; set; }
    }
}
