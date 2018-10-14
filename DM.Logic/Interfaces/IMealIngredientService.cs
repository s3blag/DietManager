using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DM.Models.ViewModels;

namespace DM.Logic.Interfaces
{
    public interface IMealIngredientService
    {
        Task<MealIngredientVM> GetMealIngredientAsync(Guid mealIgredientId);
        Task<Guid> AddMealIngredientAsync(MealIngredientCreationVM mealIngredient);
        Task<IEnumerable<MealIngredientVM>> GetMealIngredientsForMealAsync(Guid mealId);
    }
} 