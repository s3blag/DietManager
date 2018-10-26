using System.Collections.Generic;
using System.Threading.Tasks;
using DM.Models.ViewModels;

namespace DM.Logic.Interfaces
{
    public interface IMealIngredientsApiCaller
    {
        Task<IList<MealIngredientVM>> GetMealIngredientsByQueryAsync(string query);
    }
}