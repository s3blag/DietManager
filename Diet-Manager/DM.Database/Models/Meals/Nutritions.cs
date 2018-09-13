using System;

namespace DM.Logic.Models.Meals
{
    public class Nutritions
    {
        public Guid MealIngredientId { get; set; }

        public float ProteinAmount { get; set; }

        public float Carbohydrates { get; set; }

        public float Fats { get; set; }

        public float VitaminA { get; set; }

        public float VitaminC { get; set; }

        public float VitaminB6 { get; set; }

        public float VitaminD { get; set; }
    }
}