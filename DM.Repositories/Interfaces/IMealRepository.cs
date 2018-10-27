using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DM.Database;
using DM.Models.Models;
using DM.Models.Wrappers;

namespace DM.Repositories.Interfaces
{
    public interface IMealRepository : IBaseRepository<Meal>
    {
        Task<bool> AddMealMealIngredientsAsync(IEnumerable<MealMealIngredient> mealMealIngredients);
        Task<IEnumerable<Meal>> GetAllMealsAsync();
        Task<Meal> GetMealByIdAsync(Guid id);
        Task<bool> UpdateMealAsync(Meal newMealData);
        Task<IList<MealPreview>> GetMealPreviewsAsync(Guid userId, int index, int takeAmount);
        Task<IList<MealPreview>> GetMealPreviewsByQueryAsync(string query, int index, int takeAmount);
    }
}