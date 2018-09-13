using System;
using System.Collections.Generic;
using System.Text;

namespace DM.Logic.Models.Meals
{
    public class MealIngredient
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public Guid PhotoId { get; set; }

        public Guid NutritionsId { get; set; }

        public int Calories { get; set; }
    }
}
