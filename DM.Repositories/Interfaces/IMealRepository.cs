using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DM.Database;
using DM.Models;

namespace DM.Repositories.Interfaces
{
    public interface IMealRepository
    {
        Task<bool> AddMealAsync(Meal meal);
        Task<bool> DeleteMealAsync(Guid id);
        Task<IEnumerable<Meal>> GetAllMealsAsync();
        Task<MealWithIngredients> GetMealByIdAsync(Guid id);
        Task<IEnumerable<Meal>> GetMealsByUserAsync(Guid id);
        Task<bool> UpdateMealAsync(Meal newMealData);
    }
}