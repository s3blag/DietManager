using DM.Database;

namespace DM.Models.Models
{
    public class MealIngredientWithQuantity
    {
        public int Quantity { get; set; }
        public MealIngredient MealIngredient { get; set; }
    }
}
