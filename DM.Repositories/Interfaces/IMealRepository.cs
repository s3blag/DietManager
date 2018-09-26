﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DM.Database;

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
    }
}