using System;

namespace DM.Models.ViewModels
{
    public class MealIngredientVM
    {
        public Guid? Id { get; set; }

        public Guid? PhotoId { get; set; }

        public string Name { get; set; }

        public string Calories { get; set; }

        public NutritionsVM Nutritions { get; set; }
    }
}