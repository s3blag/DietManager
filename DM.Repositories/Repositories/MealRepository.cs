using DM.Database;
using DM.Models;
using DM.Repositories.Interfaces;
using LinqToDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DM.Repositories
{
    public class MealRepository : IMealRepository
    {
        public async Task<IEnumerable<Meal>> GetMealsByUserAsync(Guid id)
        {
            using (var db = new DietManagerDB())
            {
                var meals = await db.UserMeals.
                    LoadWith(um => um.Meal).
                    Where(um => um.UserId == id).
                    Select(um => um.Meal).
                    ToListAsync();

                return meals;
            }
        }

        public async Task<MealWithIngredients> GetMealByIdAsync(Guid id)
        {
            using (var db = new DietManagerDB())
            {
                var mealWithIngredients = await db.MealMealIngredients.
                    LoadWith(m => m.MealIngredient).
                    LoadWith(m => m.Meal).
                    Where(m => m.MealId == id).
                    Select(m => new Tuple<Meal, MealIngredient>(m.Meal, m.MealIngredient)).
                    ToListAsync();

                var ingredients = mealWithIngredients.Select(m => m.Item2);

                return new MealWithIngredients(mealWithIngredients.FirstOrDefault().Item1, ingredients);
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

    }
}
