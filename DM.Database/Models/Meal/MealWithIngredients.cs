using DM.Database;
using DM.Models.Models;
using System;
using System.Collections.Generic;

namespace DM.Models
{
    public class MealWithIngredients
    {
        public MealWithIngredients(Meal meal, IEnumerable<MealIngredientWithQuantity> ingredients)
        {
            Id = meal.Id;
            PhotoId = meal.PhotoId;
            Name = meal.Name;
            Calories = meal.Calories;
            Ingredients = ingredients;
        }

        public Guid Id { get; set; }

        public Guid? PhotoId { get; set; }

        public string Name { get; set; }

        public float Calories { get; set; }

        public IEnumerable<MealIngredientWithQuantity> Ingredients { get; set; }
    }
}
