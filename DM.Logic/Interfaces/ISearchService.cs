using System.Collections.Generic;
using System.Threading.Tasks;
using DM.Models.ViewModels;
using DM.Models.Wrappers;

namespace DM.Logic.Interfaces
{
    public interface ISearchService
    {
        Task<IndexedResult<IEnumerable<MealPreviewVM>>> SearchMealAsync(IndexedResult<MealSearchVM> searchArgumentsVM, int takeAmount = 10);
        Task<IndexedResult<IEnumerable<MealIngredientVM>>> SearchMealIngredientAsync(IndexedResult<MealIngredientSearchVM> searchArgumentsVM, int takeAmount = 10);
    }
}