using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DM.Database;

namespace DM.Repositories.Interfaces
{
    public interface IMealIngredientRepository
    {
        Task<bool> AddMealIngredientAsync(MealIngredient mealIngredient);
        Task<MealIngredient> GetMealIngredientByIdAsync(Guid id);
        Task<IEnumerable<MealIngredient>> GetMealIngredientsForMealAsync(Guid mealId);
    }
}