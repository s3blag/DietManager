using DM.Database;
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
    public class MealRepository : IMealRepository
    {
        public MealRepository() {}

        public async Task<Meal> GetMealByIdAsync(Guid id)
        {
            using (var db = new DietManagerDB())
            {
                var meal = await db.Meals.
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

        public async Task<bool> AddMealAsync(Meal meal)
        {
            using (var db = new DietManagerDB())
            {
                var result = await db.
                    InsertAsync(meal);

                return Convert.ToBoolean(result);
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

        public async Task<bool> UpdateMealAsync(Meal newMealData)
        {
            using (var db = new DietManagerDB())
            {
                var result = await db.
                    UpdateAsync(newMealData);

                return Convert.ToBoolean(result);
            }
        }

        public async Task<bool> DeleteMealAsync(Guid id)
        {
            using (var db = new DietManagerDB())
            {
                var result = await db.Meals.
                    Where(m => m.Id == id).
                    DeleteAsync();

                return Convert.ToBoolean(result);
            }
        }

        public async Task<IList<MealPreview>> GetMealPreviewsAsync(Guid userId, int index, int takeAmount)
        {
            using (var db = new DietManagerDB())
            {
                var mealPreviewsQuery = db.Meals.
                    Where(m => m.CreatorId == userId).
                    OrderBy(m => m.Name).
                    ThenBy(m => m.CreationDate).
                    Skip(index).
                    Take(takeAmount).
                    Select(m => new MealPreview(m.Id, m.ImageId, m.Name, (int)m.Calories, m.NumberOfUses, m.CreationDate));

                return await mealPreviewsQuery.ToListAsync();
            }
        }

        public async Task<IList<MealPreview>> GetMealPreviewsByQueryAsync(string query, int index, int takeAmount)
        {
            using (var db = new DietManagerDB())
            {
                var mealPreviewsQuery = db.Meals.
                    Where(m => m.Name.ToLower().Contains(query)).
                    OrderBy(m => m.Name).
                    ThenBy(m => m.CreationDate).
                    Skip(index).
                    Take(takeAmount).
                    Select(m => new MealPreview(m.Id, m.ImageId, m.Name, (int)m.Calories, m.NumberOfUses, m.CreationDate));

                return await mealPreviewsQuery.ToListAsync();
            }
        }
    }
}
