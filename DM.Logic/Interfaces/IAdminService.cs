using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DM.Models.ViewModels;
using DM.Models.Wrappers;

namespace DM.Logic.Interfaces
{
    public interface IAdminService
    {
        Task<bool> DeleteMealAsync(Guid mealId);
        Task<bool> DeleteMealIngredientAsync(Guid mealIngredientId);
        Task<bool> DeleteUserAsync(Guid userId);
        Task<IndexedResult<IEnumerable<UserActivityVM>>> GetUsersActivitiesAsync(IndexedResult<UserActivityVM> lastReturned, int takeAmount = Constants.DEFAULT_DB_TAKE_VALUE);
        Task<bool> MarkActivitiesAsSeenAsync(IEnumerable<int> activitiesIds);
    }
}