using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DM.Models.ViewModels;
using DM.Models.Wrappers;

namespace DM.Logic.Interfaces
{
    public interface ISearchService
    {
        Task<IndexedResult<IEnumerable<MealPreviewVM>>> SearchMealsAsync(Guid userId, IndexedResult<MealSearchVM> searchArgumentsVM, int takeAmount = Constants.DEFAULT_DB_TAKE_VALUE);
        Task<IndexedResult<IEnumerable<MealIngredientVM>>> SearchMealIngredientsAsync(IndexedResult<MealIngredientSearchVM> searchArgumentsVM, int takeAmount = Constants.DEFAULT_DB_TAKE_VALUE);
        Task<IndexedResult<IEnumerable<UserVM>>> SearchUsersAsync(Guid userId, IndexedResult<UserSearchVM> searchArgumentsVM, int takeAmount = Constants.DEFAULT_DB_TAKE_VALUE);
    }
}