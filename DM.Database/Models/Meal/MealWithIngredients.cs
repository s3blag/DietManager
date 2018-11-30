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
            ImageId = meal.ImageId;
            Name = meal.Name;
            Calories = meal.Calories;
            Ingredients = ingredients;
            Creator = meal.Creator;
            NumberOfFavouriteMarks = meal.NumberOfFavouriteMarks;
            NumberOfUses = meal.NumberOfUses;
            Description = meal.Description;
        }

        public Guid Id { get; set; }

        public Guid? ImageId { get; set; }

        public string Name { get; set; }

        public float Calories { get; set; }

        public string Description { get; set; }

        public User Creator { get; set; }

        public int NumberOfFavouriteMarks { get; set; }

        public int NumberOfUses { get; set; }

        public IEnumerable<MealIngredientWithQuantity> Ingredients { get; set; }
    }
}
