using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DM.Database;
using DM.Models.Models;

namespace DM.Repositories.Interfaces
{
    public interface IMealIngredientRepository: IBaseRepository<MealIngredient>
    {
        Task<bool> AddMealIngredientsAsync(IEnumerable<MealIngredient> mealIngredients);
        Task<bool> AddMealIngredientNutritionsAsync(Nutrition nutritions);
        Task<MealIngredient> GetMealIngredientByIdAsync(Guid id);
        Task<IEnumerable<MealIngredientWithQuantity>> GetMealIngredientsForMealAsync(Guid mealId);
        Task<ICollection<MealIngredient>> GetMealIngredientsByQueryAsync(string query, int index, int takeAmount);
        Task<Dictionary<Guid, List<MealIngredientWithQuantity>>> GetMealIngredientsForMealsAsync(IEnumerable<Guid> mealIds);
        Task<bool> MarkAsDeletedAsync(Guid mealIngredientId);
    }
}