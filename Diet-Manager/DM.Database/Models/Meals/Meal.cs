using System;
using System.Collections.Generic;
using System.Text;

namespace DM.Logic.Models.Meals
{
    public class Meal
    {
        public Guid Id { get; set; }

        public List<MealIngredient> Ingredients { get; set; }

        public Guid PhotoId { get; set; }

        public string Name { get; set; }

        public float Calories { get; set; }
    }
}
