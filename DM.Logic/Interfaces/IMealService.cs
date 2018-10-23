using System;
using System.Threading.Tasks;
using DM.Models.Models;
using DM.Models.ViewModels;
using DM.Models.Wrappers;

namespace DM.Logic.Interfaces
{
    public interface IMealService
    {
        Task<Guid> AddMealAsync(MealCreationVM mealVM, Guid userId);
        Task<MealVM> GetMealByIdAsync(Guid id);
        Task<IndexedResult<MealPreviewVM>> GetMealPreviewsAsync(Guid userId, MealPreviewVM lastReturned, int takeAmount = DbConstants.DEFAULT_DB_TAKE_VALUE);
    }
}