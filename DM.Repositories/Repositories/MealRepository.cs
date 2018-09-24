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
        private readonly IMealIngredientRepository _mealIngredientRepository;

        public MealRepository(IMealIngredientRepository mealIngredientRepository)
        {
            _mealIngredientRepository = mealIngredientRepository;
        }

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
                var ingredients = await _mealIngredientRepository.GetMealIngredientsForMealAsync(id);

                var meal = await db.Meals.
                    FirstOrDefaultAsync(m => m.Id == id);

                return new MealWithIngredients(meal, ingredients);
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
