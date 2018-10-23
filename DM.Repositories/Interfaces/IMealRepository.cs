using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DM.Database;
using DM.Models.Models;

namespace DM.Repositories.Interfaces
{
    public interface IMealRepository
    {
        Task<bool> AddMealAsync(Meal meal);
        Task<bool> AddMealMealIngredientsAsync(IEnumerable<MealMealIngredient> mealMealIngredients);
        Task<bool> DeleteMealAsync(Guid id);
        Task<IEnumerable<Meal>> GetAllMealsAsync();
        Task<Meal> GetMealByIdAsync(Guid id);
        Task<bool> UpdateMealAsync(Meal newMealData);
        Task<IList<MealPreview>> GetMealPreviewsAsync(Guid userId, MealPreview lastReturned, int takeAmount);
    }
}