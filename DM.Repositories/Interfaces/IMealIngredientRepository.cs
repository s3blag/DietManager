using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DM.Database;

namespace DM.Repositories.Interfaces
{
    public interface IMealIngredientRepository
    {
        Task<bool> AddMealIngredientAsync(MealIngredient mealIngredient);
        Task<bool> AddMealIngredientsAsync(IEnumerable<MealIngredient> mealIngredients);
        Task<bool> AddMealIngredientNutritionsAsync(IEnumerable<Nutrition> nutritions);
        Task<MealIngredient> GetMealIngredientByIdAsync(Guid id);
        Task<IEnumerable<MealIngredient>> GetMealIngredientsForMealAsync(Guid mealId);
    }
}