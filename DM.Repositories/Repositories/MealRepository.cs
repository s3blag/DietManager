﻿using DM.Database;
using DM.Models.Models;
using DM.Repositories.Interfaces;
using LinqToDB;
using LinqToDB.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DM.Repositories
{
    public class MealRepository : BaseRepository<Meal>, IMealRepository
    {

        public async Task<Meal> GetMealByIdAsync(Guid id)
        {
            using (var db = new DietManagerDB())
            {
                var meal = await db.Meals.
                    LoadWith(m => m.Creator).
                    Where(m => !m.Deleted).
                    FirstOrDefaultAsync(m => m.Id == id);

                return meal;
            }
        }

        public async Task<IEnumerable<Meal>> GetAllMealsAsync()
        {
            using (var db = new DietManagerDB())
            {
                var meals = await db.Meals.
                    ToListAsync();

                return meals;
            }
        }


        public async Task<bool> AddMealMealIngredientsAsync(
            IEnumerable<MealMealIngredient> mealMealIngredients
            )
        {
            return await Task.Run(() =>
            {
                using (var db = new DietManagerDB())
                {
                    var result = db.BulkCopy(mealMealIngredients);

                    return result.RowsCopied == mealMealIngredients.Count();
                }
            });
        }

        public async Task<ICollection<MealPreview>> GetMealPreviewsAsync(Guid userId, int index, int takeAmount)
        {
            using (var db = new DietManagerDB())
            {
                var mealPreviewsQuery = db.Meals.
                    LoadWith(m => m.Creator).
                    Where(m => m.CreatorId == userId).
                    Where(m => !m.Deleted).
                    OrderBy(m => m.Name).
                    ThenBy(m => m.CreationDate).
                    Skip(index).
                    Take(takeAmount).
                    Select(m => new MealPreview(m.Id, m.Creator, m.ImageId, m.Name, m.Calories, m.NumberOfUses, m.NumberOfFavouriteMarks, m.CreationDate));

                return await mealPreviewsQuery.ToListAsync();
            }
        }

        public async Task<ICollection<MealPreview>> GetMealPreviewsByQueryAsync(string query, int index, int takeAmount)
        {
            using (var db = new DietManagerDB())
            {
                var mealPreviewsQuery = db.Meals.
                    LoadWith(m => m.Creator).
                    Where(m => m.Name.ToLower().Contains(query)).
                    Where(m => !m.Deleted).
                    OrderBy(m => m.NumberOfFavouriteMarks).
                    ThenBy(m => m.Name).
                    ThenBy(m => m.CreationDate).
                    Skip(index).
                    Take(takeAmount).
                    Select(m => new MealPreview(m.Id, m.Creator, m.ImageId, m.Name, m.Calories, m.NumberOfUses, m.NumberOfFavouriteMarks, m.CreationDate));

                return await mealPreviewsQuery.ToListAsync();
            }
        }

        public async Task<bool> MarkAsDeletedAsync(Guid mealId)
        {
            using (var db = new DietManagerDB())
            {
                int rowsAffected = await db.Meals.
                    Where(m => m.Id == mealId).
                    Set(m => m.Deleted, true).
                    UpdateAsync();

                return rowsAffected == 1;
            }
        }
    }
}
