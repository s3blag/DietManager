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
    public class MealIngredientRepository : IMealIngredientRepository
    {
        public async Task<MealIngredient> GetMealIngredientByIdAsync(Guid id)
        {
            using (var db = new DietManagerDB())
            {
                var mealIngredient = await db.MealIngredients.
                    LoadWith(mi => mi.Nutrition).
                    LoadWith(mi => mi.Image).
                    FirstOrDefaultAsync(m => m.Id == id);

                return mealIngredient;
            }
        }

        public async Task<bool> AddMealIngredientAsync(MealIngredient mealIngredient)
        {
            using (var db = new DietManagerDB())
            {
                int result = await db.InsertAsync(mealIngredient);

                return Convert.ToBoolean(result);
            } 
        }

        public async Task<bool> AddMealIngredientsAsync(IEnumerable<MealIngredient> mealIngredients)
        {
            return await Task.Run(() =>
            {
                using (var db = new DietManagerDB())
                {
                    var result = db.BulkCopy(mealIngredients);

                    return result.RowsCopied == mealIngredients.Count();
                }
            });
        }

        public async Task<bool> AddMealIngredientNutritionsAsync(Nutrition nutritions)
        {

            using (var db = new DietManagerDB())
            {
                var result = await db.InsertAsync(nutritions);

                return Convert.ToBoolean(result);
            }
       
        }

        public async Task<IEnumerable<MealIngredientWithQuantity>> GetMealIngredientsForMealAsync(Guid mealId)
        {
            using (var db = new DietManagerDB())
            {
                var mealIngredients = await db.MealFullMealIngredients.
                    Where(m => m.MealId == mealId).
                    Select(m => new MealIngredientWithQuantity()
                    {   Quantity = m.Quantity.Value,
                        MealIngredient =new MealIngredient()
                        {
                            Id = m.MealIngredientId.Value,
                            ImageId = m.MealIngredientImageId.GetValueOrDefault(),
                            Name = m.MealIngredientName,
                            Calories = m.MealIngredientCalories.Value,
                            Nutrition = new Nutrition()
                            {
                                Protein = m.Protein.Value,
                                Carbohydrates  = m.Carbohydrates.Value,
                                Fats = m.Fats.Value,
                                VitaminA = m.VitaminA,
                                VitaminC = m.VitaminC,
                                VitaminB6 = m.VitaminB6,
                                VitaminD = m.VitaminD
                            }
                        }
                    }).
                    ToListAsync();

                return mealIngredients;
            }
        }
    }
}
