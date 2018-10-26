using DM.Logic.Interfaces;
using DM.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DM.Logic.Services
{
    public class MealIngredientsApiCaller : IMealIngredientsApiCaller
    {
        public  Task<IList<MealIngredientVM>> GetMealIngredientsByQueryAsync(string query)
        {
            throw new NotImplementedException();
        }

    }
}
