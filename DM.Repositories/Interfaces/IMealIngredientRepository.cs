﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DM.Database;
using DM.Models.Models;

namespace DM.Repositories.Interfaces
{
    public interface IMealIngredientRepository
    {
        Task<bool> AddMealIngredientAsync(MealIngredient mealIngredient);
        Task<bool> AddMealIngredientsAsync(IEnumerable<MealIngredient> mealIngredients);
        Task<bool> AddMealIngredientNutritionsAsync(Nutrition nutritions);
        Task<MealIngredient> GetMealIngredientByIdAsync(Guid id);
        Task<IEnumerable<MealIngredientWithQuantity>> GetMealIngredientsForMealAsync(Guid mealId);
        Task<IList<MealIngredient>> GetMealIngredientsByQueryAsync(string query, int index, int takeAmount);
    }
}